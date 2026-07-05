using System.Diagnostics;
using Raylib_cs;

namespace GeometryDash.Engine.Core
{
  public class Engine
  {
    private bool isRunning;
    private TimeStep timeStep;

    public void Start()
    {
      timeStep = new TimeStep((int)(GameSettings.targetFrameRate));
      timeStep.Start();
      isRunning = true;

      Run();
    }

    private void Run()
    {
      while (isRunning)
      {
        int updateTicks = timeStep.GetRequiredUpdateTicks();

        for (int i = 0; i < updateTicks; i++)
        {
          Update(timeStep.FixedDeltaTime);
        }

        Render();
      }
    }

    private void Update(float deltaTime)
    {
      //Physics, etc.
    }

    private void Render()
    {
      //Drawing logic
    }
  }
}
