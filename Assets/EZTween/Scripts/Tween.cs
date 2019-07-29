using System;

namespace ENVCode.EZTween
{
    public class Tween
    {
        #region Properties
        public float Progress
        {
            get;
            private set;
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

        private Func<float, float> m_InterpolationFunc;
        private Action<float> m_OnUpdate;
        private Action m_OnPause;
        private Action m_OnComplete;
        private Action m_OnStop;
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
            if (m_OnPause != null)
                m_OnPause.Invoke();
        }

        public void Stop()
        {
            if (!Playing)
                return;

            Progress = 1f;
            Playing = false;
            if (m_OnStop != null)
                m_OnStop.Invoke();
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

        internal void Tick(float dt)
        {
            if (!Playing)
                return;

            Progress += dt;
            float value = m_InterpolationFunc.Invoke(Progress / Duration);
            m_OnUpdate.Invoke(value);
            if (Progress >= Duration)
            {
                if (m_OnComplete != null)
                {
                    m_OnComplete.Invoke();
                }
                Stop();
            }
        }
    }
}