using GeometryDash.Engine.Entities;
using GeometryDash.Engine.Core;

namespace GeometryDash.Engine.Physics
{
  public class CollisionEngine
  {
    public bool CheckOverlap(Entities.GameObject obj, Entities.PlayerCube player)
    {
      float playerLeft = player.PosX;
      float playerRight = player.PosX + player.Width;
      float playerTop = player.PosY;
      float playerBottom = player.PosY + player.Height;

      if (obj.Type == Entities.GameObject.ObjectType.Spike)
      {
        float playerShrinkWidth = player.Width * 0.35f;
        float playerShrinkHeight = player.Height * 0.35f; 

        playerLeft += playerShrinkWidth;
        playerRight -= playerShrinkWidth;
        playerTop += playerShrinkHeight;
        playerBottom -= playerShrinkHeight;
      }

      float objLeft = obj.PosX;
      float objRight = obj.PosX + obj.SizeX;
      float objTop = obj.PosY;
      float objBottom = obj.PosY + obj.SizeY;

      if (obj.Type == Entities.GameObject.ObjectType.Spike)
      {
        float spikeShrinkWidth = obj.SizeX * 0.30f;
        float spikeShrinkHeight = obj.SizeY * 0.50f;

        objLeft += spikeShrinkWidth;
        objRight -= spikeShrinkWidth;
        objTop += spikeShrinkHeight;
      }

      if (playerRight < objLeft) return false;
      if (playerLeft > objRight) return false;
      if (playerBottom < objTop) return false;
      if (playerTop > objBottom) return false;

      return true;
    }

    public void ResolveCollision(Entities.GameObject obj, Entities.PlayerCube player)
    {
      if (!CheckOverlap(obj, player)) return;

      switch (obj.Type)
      {
        case Entities.GameObject.ObjectType.SolidBlock:
          bool isFalling = player.VelocityY >= 0;
          bool wasAbove = (player.PosY + player.Height) - player.VelocityY <= obj.PosY + 1f;

          if (isFalling && wasAbove)
          {
            player.PosY = obj.PosY - player.Height; 
            player.VelocityY = 0f;                  
            player.IsGrounded = true;
          } else player.IsDead = true;

          break;

        case Entities.GameObject.ObjectType.Spike:
          player.IsDead = true;
          break;

        case Entities.GameObject.ObjectType.JumpPad:
          player.VelocityY = -Core.GameSettings.jumpPadForce;
          player.IsGrounded = false;
          break;
      }
    }
  }
}
