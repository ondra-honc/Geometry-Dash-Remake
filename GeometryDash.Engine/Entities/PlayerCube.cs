using GeometryDash.Engine.Core;

namespace GeometryDash.Engine.Entities
{
  internal class PlayerCube
  {
    public float PosX {  get; set; }
    public float PosY { get; set; }
    public float VelocityY {  get; set; }
    public bool IsGrounded { get; private set; }
    public float Width { get; } = 40f;
    public float Height { get; } = 40f;

    private const float Gravity = 2000f;    
    private const float JumpForce = -600f;  
    private const float FloorY = 350f;

    public PlayerCube(float startX, float startY)
    {
      PosX = startX;
      PosY = startY;
      VelocityY = 0;
      IsGrounded = false;
    }
  }
}
