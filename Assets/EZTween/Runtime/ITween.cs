namespace ENVCode.EZTween
{
    public interface ITween
    {
        void Tick(float deltaTime);
        bool IsPlaying { get; }
        void Pause();
    }
}