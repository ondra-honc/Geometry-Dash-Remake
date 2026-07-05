using GeometryDash.Engine.Entities;
using GeometryDash.Engine.World;
using Raylib_cs;
using System.Diagnostics;
using System.Numerics;

namespace GeometryDash.Engine.Core
{
  public class Engine
  {
    private bool isRunning;
    private TimeStep timeStep;
    private World.LevelManager levelManager;
    private World.LevelStreamer levelStreamer;
    private float cameraX = 0f;
    private const int Size = 40;
    private Entities.PlayerCube cube;

    public void Start()
    {
      Raylib.InitWindow(1280,720, "Geometry Dash Remake");
      Raylib.ToggleBorderlessWindowed();
      Raylib.SetTargetFPS((int)(GameSettings.targetFrameRate));
      
      levelManager = new World.LevelManager();
      levelManager.LoadLevel("level1.gdl");

      Entities.ObjectPool pool = new Entities.ObjectPool();
      levelStreamer = new World.LevelStreamer();
      levelStreamer.Initialize(pool);

      cube = new Entities.PlayerCube(0f, 350f);
      
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
      cameraX += 300f * deltaTime;
      levelStreamer.UpdateStreaming(levelManager.Blueprints, cameraX, 1280f);
    }

    private void Render()
    {
      Raylib.BeginDrawing();
      Raylib.ClearBackground(Color.SkyBlue);

      int screenWidth = Raylib.GetScreenWidth();
      int screenHeight = Raylib.GetScreenHeight();

      int floorHeight = (int)(screenHeight * 0.20f);
      int floorY = screenHeight - floorHeight; 

      Raylib.DrawRectangle(0, floorY, screenWidth, floorHeight, Color.DarkBlue);
      Raylib.DrawLine(0, floorY, screenWidth, floorY, Color.White);

      foreach (var obj in levelStreamer.ActiveObjects)
      {
        int screenX = (int)(obj.PosX - cameraX);
        int screenY = (int)obj.PosY;

        if (obj.Type == GameObject.ObjectType.SolidBlock)
        {
          Raylib.DrawRectangle(screenX, screenY, Size, Size, Color.Gray);
          Raylib.DrawRectangleLines(screenX, screenY, Size, Size, Color.White);
        }
        else if (obj.Type == GameObject.ObjectType.Spike)
        {
          Vector2 top = new Vector2(screenX + (Size / 2f), screenY);
          Vector2 bottomLeft = new Vector2(screenX, screenY + Size);
          Vector2 bottomRight = new Vector2(screenX + Size, screenY + Size);

          Raylib.DrawTriangle(top, bottomLeft, bottomRight, Color.Red);
        }
        else if (obj.Type == GameObject.ObjectType.JumpPad)
        {
          //TODO
        }
      }
      int playerScreenX = 200;
      int playerScreenY = (int)cube.PosY;

      Raylib.DrawRectangle(playerScreenX, playerScreenY, Size, Size, Color.Green);
      Raylib.EndDrawing();
    }
  }
}
