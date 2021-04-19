namespace ENVCode.EZTween
{
    public interface ITween
    {
        void Tick(float deltaTime);
        bool Playing { get; }
        bool Stopped { get; }
        void Stop();
    }
}