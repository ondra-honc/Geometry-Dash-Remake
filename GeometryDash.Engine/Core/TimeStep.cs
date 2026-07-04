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

    
  }
}