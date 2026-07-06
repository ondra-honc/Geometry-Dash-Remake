using GeometryDash.Engine.Core;
using Raylib_cs;

namespace GeometryDash.Engine.Entities
{
  public class PlayerCube
  {
    public Texture2D Texture { get; private set; }
    public float PosX {  get; set; }
    public float PosY { get; set; }
    public float VelocityY {  get; set; }
    public bool IsGrounded { get; internal set; }
    public bool IsDead { get; set; }
    public float Width { get; } = 80f;
    public float Height { get; } = 80f;

    public PlayerCube(float startX, float startY, Texture2D playerTexture)
    {
      Texture = playerTexture;
      PosX = startX;
      PosY = startY;
      VelocityY = 0;
      IsGrounded = false;
    }
  }
}
