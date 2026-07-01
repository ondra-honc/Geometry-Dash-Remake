using GeometryDash.Engine.Core;

namespace GeometryDash.Engine.Entities
{
  internal class PlayerCube
  {
    public float posX {  get; set; }
    public float posY { get; set; }
    public float velocityY {  get; set; }
    public bool isGrounded { get; private set; }
    public float width { get; } = 40f;
    public float height { get; } = 40f;

    private const float Gravity = 2000f;    
    private const float JumpForce = -600f;  
    private const float FloorY = 350f;

    public PlayerCube(float startX, float startY)
    {
      posX = startX;
      posY = startY;
      velocityY = 0;
      isGrounded = false;
    }
  }
}
