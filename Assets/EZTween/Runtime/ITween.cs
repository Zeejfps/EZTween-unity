namespace ENVCode.EZTween
{
    public interface ITween
    {
        void Tick(float deltaTime);
        bool IsPlaying { get; }
        bool IsStopped { get; }
        void Stop();
    }
}