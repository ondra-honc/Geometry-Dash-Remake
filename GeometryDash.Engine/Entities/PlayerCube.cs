using GeometryDash.Engine.Core;

namespace GeometryDash.Engine.Entities
{
  public class PlayerCube
  {
    public float PosX {  get; set; }
    public float PosY { get; set; }
    public float VelocityY {  get; set; }
    public bool IsGrounded { get; internal set; }
    public bool IsDead { get; set; }
    public float Width { get; } = 40f;
    public float Height { get; } = 40f;

    public PlayerCube(float startX, float startY)
    {
      PosX = startX;
      PosY = startY;
      VelocityY = 0;
      IsGrounded = false;
    }
  }
}
