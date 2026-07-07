namespace GeometryDash.Engine.Core
{
  public class GameSettings
  {
    public const int windowWidth = 1280;
    public const int windowHeight = 720;

    public const float targetFrameRate = 60f;
    public const float targetUpdate = 1f / targetFrameRate;
    public const float cameraSpeed = 700f;

    public const float cubeHorizontalSpeed = 450f;
    public const float gravityForce = 5300f;
    public const float jumpImpulse = -1500f;
    public const float jumpPadForce = 1500f;
    public const float floorFloat = 0.28333f;
  }
}
