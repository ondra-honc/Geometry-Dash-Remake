using System.Diagnostics;

namespace GeometryDash.Engine.Core
{
  public class TimeStep
  {
    private readonly Stopwatch _stopwatch = new Stopwatch();
    private double _previousTime;
    private double _accumulatedTime;
    private readonly double _targetFrameTime;

    public TimeStep(int targetFps)
    {
      _targetFrameTime = 1.0 / targetFps;
    }

    public void Start()
    {
      _stopwatch.Start();
      _previousTime = _stopwatch.Elapsed.TotalSeconds;
      _accumulatedTime = 0.0;
    }

    public int GetRequiredUpdateTicks()
    {
      double currentTime = _stopwatch.Elapsed.TotalSeconds;
      double elapsedTime = currentTime - _previousTime;
      _previousTime = currentTime;

      _accumulatedTime += elapsedTime;

      int ticks = (int)(_accumulatedTime / _targetFrameTime);
      _accumulatedTime -= ticks * _targetFrameTime;

      return ticks;
    }

    public float GetAlpha()
    {
      return (float)(_accumulatedTime / _targetFrameTime);
    }

    public float FixedDeltaTime => (float)_targetFrameTime;
  }
}