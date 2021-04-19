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

        public bool Playing
        {
            get;
            private set;
        }

        public bool Stopped
        {
            get { return !Playing; }
        }

        public bool Paused
        {
            get { return !Playing && !Stopped; }
        }

        Func<float, float> m_InterpolationFunc;
        Action<float> m_OnUpdate;
        Action m_OnPause;
        Action m_OnComplete;
        Action m_OnStop;
        float m_Progress;
        float m_Time;
        #endregion

        #region Constructor
        public Tween(Func<float, float> interpolationFunc, float duration, Action<float> onUpdate)
        {
            m_InterpolationFunc = interpolationFunc;
            m_OnUpdate = onUpdate;
            Duration = duration;
        }
        #endregion

        #region Public Methods
        public void Restart()
        {
            m_Time = 0f;
            Progress = 0f;
            Playing = false;
            Play();
        }

        public void Play()
        {
            if (Playing)
                return;

            Playing = true;
            EZTween.Play(this);
        }

        public void Play(object key)
        {
            if (Playing)
                return;

            Playing = true;
            EZTween.Play(key, this);
        }

        public void Pause()
        {
            if (!Playing)
                return;

            Playing = false;
            m_OnPause?.Invoke();
        }

        public void Stop()
        {
            if (!Playing)
                return;

            Progress = 1f;
            Playing = false;
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
        #endregion

        public void Tick(float dt)
        {
            if (!Playing)
                return;

            m_Time += dt;
            Progress = m_Time / Duration;
            var t = m_InterpolationFunc.Invoke(Progress);
            m_OnUpdate.Invoke(t);
            if (m_Time >= Duration)
            {
                m_OnComplete?.Invoke();
                Stop();
            }
        }
    }
}