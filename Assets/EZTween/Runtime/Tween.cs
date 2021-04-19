using System;

namespace ENVCode.EZTween
{
    public class Tween : ITween
    {
        #region Properties
        public float Progress
        {
            get => m_Progress;
            private set
            {
                m_Progress = value;
                if (m_Progress > 1f)
                    m_Progress = 1f;
            }
        }

        float _duration;
        public float Duration
        {
            get { return _duration; }
            private set
            {
                _duration = Math.Max(value, 0);
            }
        }

        public bool IsPlaying
        {
            get;
            private set;
        }

        public bool IsStopped
        {
            get { return !IsPlaying; }
        }

        public bool Paused
        {
            get { return !IsPlaying && !IsStopped; }
        }

        Func<float, float> m_EasingFunc;
        Action<float> m_OnUpdate;
        Action m_OnPause;
        Action m_OnComplete;
        Action m_OnStop;
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
        public void Restart()
        {
            m_Time = 0f;
            Progress = 0f;
            IsPlaying = false;
            Play();
        }

        public void Play()
        {
            if (IsPlaying)
                return;

            IsPlaying = true;
            EZTween.Play(this);
        }

        public void Play(object key)
        {
            if (IsPlaying)
                return;

            IsPlaying = true;
            EZTween.Play(key, this);
        }

        public void Pause()
        {
            if (!IsPlaying)
                return;

            IsPlaying = false;
            m_OnPause?.Invoke();
        }

        public void Stop()
        {
            if (!IsPlaying)
                return;

            Progress = 1f;
            IsPlaying = false;
            m_OnStop?.Invoke();
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

        public Tween OnStop(Action onStop)
        {
            m_OnStop = onStop;
            return this;
        }
        
        public void Tick(float dt)
        {
            if (!IsPlaying)
                return;

            m_Time += dt;
            Progress = m_Time / Duration;
            var t = m_EasingFunc.Invoke(Progress);
            m_OnUpdate.Invoke(t);
            if (m_Time >= Duration)
            {
                m_OnComplete?.Invoke();
                Stop();
            }
        }
        
        #endregion
    }
}