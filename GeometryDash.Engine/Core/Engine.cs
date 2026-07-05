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
      Raylib.InitWindow(1280,720, "Geometry Dash Remake");
      Raylib.ToggleBorderlessWindowed();
      Raylib.SetTargetFPS((int)(GameSettings.targetFrameRate));
      
      timeStep = new TimeStep((int)(GameSettings.targetFrameRate));
      timeStep.Start();
      isRunning = true;

      Run();
    }

    private void Run()
    {
      while (isRunning && !Raylib.WindowShouldClose())
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
      Raylib.BeginDrawing();
      Raylib.ClearBackground(Color.Blank);

      //Draw

      Raylib.EndDrawing();
    }
  }
}
