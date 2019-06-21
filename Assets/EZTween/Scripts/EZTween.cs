using System;
using System.Collections.Generic;
using UnityEngine;

namespace ENVCode
{
    public partial class EZTween : MonoBehaviour
    {
        private static EZTween s_Instance;
        public static EZTween Instance {
            get {
                if (s_Instance == null) {
                    Init();
                }
                return s_Instance;
            }
        }

        public static void Init()
        {
            if (s_Instance != null)
                return;

            GameObject obj = new GameObject("EZTween");
            obj.hideFlags = HideFlags.HideAndDontSave;
            s_Instance = obj.AddComponent<EZTween>();
        }

        public static void Play(Tween tween)
        {
            Instance.tweens.Add(tween);
        }

        private HashSet<Tween> tweens = new HashSet<Tween>();

        private void Update()
        {
            List<Tween> copy = new List<Tween>(tweens);
            foreach (Tween tween in copy) {
                tween.Tick(Time.deltaTime);
                if (tween.Completed) {
                    tweens.Remove(tween);
                }
            }
        }
    }

    public class Tween
    {
        private Func<float, float> m_InterpolationFunc;
        private Action<float> m_OnUpdate;
        private Action m_OnComplete;

        public float Progress { get; private set; }
        public float Duration { get; private set; }
        public bool Completed { get; private set; }
        public bool Playing { get; private set; }
        public bool Paused { get; private set; }

        public Tween(Func<float, float> interpolationFunc, float duration, Action<float> onUpdate)
        {
            m_InterpolationFunc = interpolationFunc;
            m_OnUpdate = onUpdate;
            Duration = duration;
        }

        public void Restart()
        {
            Progress = 0f;
            Completed = false;
            Playing = false;
            Play();
        }

        public void Play()
        {
            if (Playing || Completed)
                return;

            Paused = false;
            Playing = true;
            EZTween.Play(this);
        }

        public void Pause()
        {
            if (Playing)
                Paused = true;
        }

        public void Stop()
        {
            Stop(false);
        }

        public void Stop(bool triggerOnComplete)
        {
            if (!Playing || Completed)
                return;

            Progress = 1f;
            Playing = false;
            Paused = false;
            Completed = true;
            if (triggerOnComplete && m_OnComplete != null)
                m_OnComplete.Invoke();
        }

        internal void Tick(float dt)
        {
            if (Completed || Paused)
                return;

            Progress += dt;
            float value = m_InterpolationFunc.Invoke(Progress / Duration);
            m_OnUpdate.Invoke(value);
            if (Progress >= Duration) {
                Stop(true);
            }
        }

        public Tween OnComplete(Action onComplete)
        {
            m_OnComplete = onComplete;
            return this;
        }
    } 
}