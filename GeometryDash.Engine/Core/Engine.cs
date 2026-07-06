using GeometryDash.Engine.Entities;
using GeometryDash.Engine.World;
using Raylib_cs;
using System.Diagnostics;
using System.Numerics;
using GeometryDash.Engine.Physics;

namespace GeometryDash.Engine.Core
{
  public class Engine
  {
    private bool isRunning;
    private TimeStep timeStep;
    private World.LevelManager levelManager;
    private World.LevelStreamer levelStreamer;
    private float cameraX = 0f;
    private const int Size = 80;
    private Entities.PlayerCube cube;
    private Physics.CollisionEngine collisionEngine;
    private int attemptCounter = 1;
    private Texture2D cubeTexture;

    public int screenWidth;
    public int screenHeight;
    public int floorHeight;
    public int floorY;

    public void Start()
    {
      Raylib.InitWindow(1280,720, "Geometry Dash Remake");
      Raylib.ToggleBorderlessWindowed();

      screenWidth = Raylib.GetScreenWidth();
      screenHeight = Raylib.GetScreenHeight();
      floorHeight = (int)(screenHeight * GameSettings.floorFloat);
      floorY = screenHeight - floorHeight;

      collisionEngine = new Physics.CollisionEngine();

      levelManager = new World.LevelManager();
      levelManager.LoadLevel("Assets/Levels/level1.gdl");

      Entities.ObjectPool pool = new Entities.ObjectPool();
      levelStreamer = new World.LevelStreamer();
      levelStreamer.Initialize(pool);


      cubeTexture = Raylib.LoadTexture(UserSettings.cubeTextureString);
      cube = new Entities.PlayerCube(0f, floorY - Size, cubeTexture);
      cube.IsGrounded = true;
      
      timeStep = new TimeStep((int)(GameSettings.targetFrameRate));
      timeStep.Start();
      isRunning = true;

      Run();

      Raylib.UnloadTexture(cubeTexture);
    }

    private void ResetLevel()
    {
      attemptCounter++;

      cube.IsDead = false;
      cube.VelocityY = 0f;
      cube.IsGrounded = true;
      cube.PosY = floorY - Size;

      cameraX = 0f;
      cube.PosX = cameraX + 300f;

      levelStreamer.Reset();
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

        float alpha = timeStep.GetAlpha(); 

        Render(alpha);
      }
    }

    private void Update(float deltaTime)
    {
      if (cube.IsDead)
      {
        ResetLevel();
        return;
      }

      cameraX += 400f * deltaTime;
      cube.PosX = cameraX + 500f;

      levelStreamer.UpdateStreaming(levelManager.Blueprints, cameraX, (float)(Raylib.GetScreenWidth()));

      cube.IsGrounded = false;

      cube.VelocityY += GameSettings.gravityForce * deltaTime;
      cube.PosY += cube.VelocityY * deltaTime;

      float groundLevelY = floorY - Size;
      if (cube.PosY >= groundLevelY)
      {
        cube.PosY = groundLevelY; 
        cube.VelocityY = 0f;
        cube.IsGrounded = true;
      }

      foreach (var obj in levelStreamer.ActiveObjects)
      {
        obj.SizeX = Size;
        obj.SizeY = Size;

        if (collisionEngine.CheckOverlap(obj, cube))
        {
          collisionEngine.ResolveCollision(obj, cube);
        }
      }
      
      if (cube.IsGrounded && (Raylib.IsKeyDown(KeyboardKey.Space) || Raylib.IsMouseButtonDown(MouseButton.Left)))
      {
        cube.VelocityY = GameSettings.jumpImpulse;
        cube.IsGrounded = false;
      }
    }

    private void Render(float alpha)
    {
      Raylib.BeginDrawing();
      Raylib.ClearBackground(Color.SkyBlue);

      float smoothCameraX = cameraX + (400f * timeStep.FixedDeltaTime * alpha);

      Raylib.DrawText($"Attempt: {attemptCounter}", -(int)(smoothCameraX) + 650, floorY - 350, 60, Color.White);

      
      Raylib.DrawRectangle(0, floorY, screenWidth, floorHeight, Color.DarkBlue);
      Raylib.DrawLine(0, floorY, screenWidth, floorY, Color.White);

      foreach (var obj in levelStreamer.ActiveObjects)
      {
        int screenX = (int)Math.Round(obj.PosX - smoothCameraX);
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

      float smoothPlayerX = smoothCameraX + 500f;
      int playerScreenX = (int)Math.Round(smoothPlayerX - smoothCameraX);
      int playerScreenY = (int)cube.PosY;

      float halfSize = Size / 2f;
      float playerCenterX = playerScreenX + halfSize;
      float playerCenterY = playerScreenY + halfSize;

      Raylib.DrawTexturePro(
        cube.Texture,
        new Rectangle(0, 0, cube.Texture.Width, cube.Texture.Height),
        new Rectangle(playerCenterX, playerCenterY, Size, Size), 
        new Vector2(halfSize, halfSize),                      
        0f,
        Color.White
      );

      Raylib.EndDrawing();
    }
  }
}
