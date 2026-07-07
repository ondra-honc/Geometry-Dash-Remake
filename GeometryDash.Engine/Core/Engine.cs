using GeometryDash.Engine.Entities;
using GeometryDash.Engine.Physics;
using GeometryDash.Engine.World;
using static GeometryDash.Engine.Shared.Enums;
using Raylib_cs;
using System.Diagnostics;
using System.Numerics;

namespace GeometryDash.Engine.Core
{
  public class Engine
  {
    private bool isRunning;
    private TimeStep timeStep;
    private LevelManager levelManager;
    private LevelStreamer levelStreamer;
    private float cameraX = 0f;
    private const int Size = 80;
    private PlayerCube cube;
    private CollisionEngine collisionEngine;
    private int attemptCounter = 1;
    private Texture2D cubeTexture;
    private float rotationAngle = 0f;
    private float currentRotationSpeed = 350f;
    private GameState currentState = GameState.MainMenu;
    private Rectangle playButtonRec;
    private bool isHoveringPlay = false;

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

      float btnWidth = 240f;
      float btnHeight = 90f;
      float btnX = (screenWidth - btnWidth) / 2f;
      float btnY = (screenHeight - btnHeight) / 2f;

      playButtonRec = new Rectangle(btnX, btnY, btnWidth, btnHeight);

      collisionEngine = new CollisionEngine();

      levelManager = new LevelManager();
      levelManager.LoadLevel("Assets/Levels/level1.gdl");

      ObjectPool pool = new ObjectPool();
      
      levelStreamer = new LevelStreamer();
      levelStreamer.Initialize(pool);

      cubeTexture = Raylib.LoadTexture(UserSettings.cubeTextureString);
      cube = new PlayerCube(0f, floorY - Size, cubeTexture);
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

      rotationAngle = 0f;

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
      switch (currentState)
      {
        case GameState.MainMenu:
          UpdateMainMenu();
          break;

        case GameState.Playing:
          UpdateGameplay(deltaTime);
          break;
      }
    }
    
    private void UpdateGameplay(float deltaTime)
    {
      if (cube.IsDead)
      {
        ResetLevel();
        return;
      }

      cameraX += 700f * deltaTime;
      cube.PosX = cameraX + 500f;

      levelStreamer.UpdateStreaming(levelManager.Blueprints, cameraX, (float)(Raylib.GetScreenWidth()));

      if (!cube.IsGrounded)
      {
        rotationAngle += currentRotationSpeed * deltaTime;
        rotationAngle %= 360f; 
      }

      bool wasAirborneLastFrame = !cube.IsGrounded;

      cube.IsGrounded = false;

      cube.VelocityY += GameSettings.gravityForce * deltaTime;
      cube.PosY += cube.VelocityY * deltaTime;

      float groundLevelY = floorY - Size;
      if (cube.PosY >= groundLevelY)
      {
        cube.PosY = groundLevelY; 
        cube.VelocityY = 0f;

        if (!cube.IsGrounded)
        {
          rotationAngle = MathF.Round(rotationAngle / 90f) * 90f;
          rotationAngle %= 360f;
        }

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

      if (cube.IsGrounded)
      {
        float targetAngle = MathF.Round(rotationAngle / 90f) * 90f;

        rotationAngle += (targetAngle - rotationAngle) * 40f * deltaTime;

        if (MathF.Abs(targetAngle - rotationAngle) < 0.1f)
        {
          rotationAngle = targetAngle % 360f;
        }
      }

      if (cube.IsGrounded && (Raylib.IsKeyDown(KeyboardKey.Space) || Raylib.IsMouseButtonDown(MouseButton.Left)))
      {
        cube.VelocityY = GameSettings.jumpImpulse;
        cube.IsGrounded = false;

        float airTime = MathF.Abs(2f * GameSettings.jumpImpulse / GameSettings.gravityForce);
        float estimatedTotalRotation = 350f * airTime;
        float perfectTargetRotation = MathF.Round(estimatedTotalRotation / 90f) * 90f;
        if (perfectTargetRotation < 90f) perfectTargetRotation = 180f;

        currentRotationSpeed = perfectTargetRotation / airTime;
      }
    }

    private void UpdateMainMenu()
    {
      Vector2 mousePos = Raylib.GetMousePosition();

      isHoveringPlay = Raylib.CheckCollisionPointRec(mousePos, playButtonRec);

      if (isHoveringPlay && Raylib.IsMouseButtonDown(MouseButton.Left))
      {
        cameraX = 0f;
        cube.PosX = cameraX + 300f;
        cube.PosY = floorY - Size;
        cube.VelocityY = 0f;
        cube.IsGrounded = true;
        rotationAngle = 0f;
        currentState = GameState.Playing;
      }
    }

    private void Render(float alpha)
    {
      Raylib.BeginDrawing();
      switch (currentState)
      {
        case GameState.MainMenu:
          RenderMainMenu();
          break;

        case GameState.Playing:
          RenderGameplay(alpha); 
          break;
      }
      Raylib.EndDrawing();
    }
    
    private void RenderGameplay(float alpha)
    {
      Raylib.ClearBackground(Color.SkyBlue);

      float smoothCameraX = cameraX + (700f * timeStep.FixedDeltaTime * alpha);

      Raylib.DrawText($"Attempt: {attemptCounter}", -(int)(smoothCameraX) + 650, floorY - 350, 60, Color.White);

      
      Raylib.DrawRectangle(0, floorY, screenWidth, floorHeight, Color.DarkBlue);
      Raylib.DrawLine(0, floorY, screenWidth, floorY, Color.White);

      foreach (var obj in levelStreamer.ActiveObjects)
      {
        int screenX = (int)Math.Round(obj.PosX - smoothCameraX);
        int screenY = (int)obj.PosY;

        if (obj.Type == ObjectType.SolidBlock)
        {
          Raylib.DrawRectangle(screenX, screenY, Size, Size, Color.Gray);
          Raylib.DrawRectangleLines(screenX, screenY, Size, Size, Color.White);
        }
        else if (obj.Type == ObjectType.Spike)
        {
          Vector2 top = new Vector2(screenX + (Size / 2f), screenY);
          Vector2 bottomLeft = new Vector2(screenX, screenY + Size);
          Vector2 bottomRight = new Vector2(screenX + Size, screenY + Size);

          Raylib.DrawTriangle(top, bottomLeft, bottomRight, Color.Red);
        }
        else if (obj.Type == ObjectType.JumpPad)
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

      float smoothRotationAngle = rotationAngle;
      if (!cube.IsGrounded)
      {
        // Predict the rotation between physics ticks for flawless smoothness
        smoothRotationAngle = rotationAngle + (currentRotationSpeed * timeStep.FixedDeltaTime * alpha);
        smoothRotationAngle %= 360f;
      }

      Raylib.DrawTexturePro(
        cube.Texture,
        new Rectangle(0, 0, cube.Texture.Width, cube.Texture.Height),
        new Rectangle(playerCenterX, playerCenterY, Size, Size), 
        new Vector2(halfSize, halfSize),                      
        smoothRotationAngle,
        Color.White
      );
    }

    private void RenderMainMenu()
    {
      string titleText = "GEOMETRY DASH";
      int titleFontSize = 70;
      int titleWidth = Raylib.MeasureText(titleText, titleFontSize);
      int titleX = (screenWidth - titleWidth) / 2;
      int titleY = (int)(screenHeight * 0.25f); 

      Raylib.DrawText(titleText, titleX, titleY, titleFontSize, Color.White);
      Color buttonColor = isHoveringPlay ? Color.Lime : Color.Green;
      Raylib.DrawRectangleRec(playButtonRec, buttonColor);
      Raylib.DrawRectangleLinesEx(playButtonRec, 4, Color.White);

      string btnText = "PLAY";
      int btnFontSize = 40;
      int btnTextWidth = Raylib.MeasureText(btnText, btnFontSize);
      int btnTextX = (int)(playButtonRec.X + (playButtonRec.Width - btnTextWidth) / 2f);
      int btnTextY = (int)(playButtonRec.Y + (playButtonRec.Height - btnFontSize) / 2f);

      Raylib.DrawText(btnText, btnTextX, btnTextY, btnFontSize, Color.White);
    }
  }
}
