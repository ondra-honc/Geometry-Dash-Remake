namespace GeometryDash.Engine.Core
{
  public class GameSettings
  {
    public const int windowWidth = 1280;
    public const int windowHeight = 720;

    public const float targetFrameRate = 60f;
    public const float targetUpdate = 1f / targetFrameRate;

    public const float cubeHorizontalSpeed = 450f;
    public const float gravityForce = 2000f;
    public const float jumpImpulse = -600f;
    public const float floorHeight = 350f;
    public const float jumpPadForce = 1500f;
  }
}
