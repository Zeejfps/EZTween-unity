using System;
using UnityEngine;

namespace ENVCode.EZTween
{
    public partial class Tween : ITween
    {
        #region Properties
        public float Progress
        {
            get => m_Progress;
            set => m_Progress = Mathf.Clamp01(value);
        }

        float _duration;
        public float Duration
        {
            get => _duration;
            set => _duration = Math.Max(value, 0);
        }

        public bool IsPlaying { get; private set; }
        
        Func<float, float> m_EasingFunc;
        Action<float> m_OnUpdate;
        Action m_OnComplete;
        Action m_OnPause;
        float m_Progress;
        float m_Time;
        #endregion

        #region Constructor
        public Tween(Func<float, float> easingFunc, float duration, Action<float> onUpdate)
        {
            m_EasingFunc = easingFunc;
            m_OnUpdate = onUpdate;
            Duration = duration;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Starts playback of the tween.
        /// If the tween is already playing the playback is restarted.
        /// </summary>
        /// <param name="key">Optional</param>
        public void Play(object key = null)
        {
            if (IsPlaying)
            {
                m_Time = 0;
                Progress = 0;
            }
            
            IsPlaying = true;
            EZTween.Play(key, this);
            OnPlay();
        }

        /// <summary>
        /// Pauses the playback of the tween.
        /// </summary>
        public void Pause()
        {
            if (!IsPlaying)
                return;

            IsPlaying = false;
            OnPause();
        }

        public Tween OnPause(Action onPause)
        {
            m_OnPause = onPause;
            return this;
        }

        public Tween OnComplete(Action onComplete)
        {
            m_OnComplete = onComplete;
            return this;
        }
        
        public void Tick(float dt)
        {
            if (!IsPlaying)
                return;

            m_Time += dt;
            Progress = m_Time / Duration;
            var t = m_EasingFunc.Invoke(Progress);
            OnUpdate(t);
            if (m_Time >= Duration)
            {
                IsPlaying = false;
                OnComplete();
            }
        }
        
        #endregion

        #region Lifecycle Methods
        
        protected virtual void OnPlay()
        {
            
        }
        
        protected virtual void OnUpdate(float progress)
        {
            m_OnUpdate.Invoke(progress);
        }

        protected virtual void OnPause()
        {
            m_OnPause?.Invoke();
        }
        
        protected virtual void OnComplete()
        {
            m_OnComplete?.Invoke();
            Progress = 0;
            m_Time = 0;
        }
        
        #endregion
    }
}