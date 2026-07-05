using GeometryDash.Engine.Entities;
using GeometryDash.Engine.Core;

namespace GeometryDash.Engine.Physics
{
  public class CollisionEngine
  {
    public bool CheckOverlap(Entities.GameObject obj, Entities.PlayerCube player)
    {
      float objLeft = obj.PosX;
      float objRight = obj.PosX + obj.SizeX;
      float objTop = obj.PosY;
      float objBottom = obj.PosY + obj.SizeY;

      if (obj.Type == Entities.GameObject.ObjectType.Spike)
      {
        float shrinkWidth = obj.SizeX * 0.30f;  
        float shrinkHeight = obj.SizeY * 0.50f; 

        objLeft += shrinkWidth;
        objRight -= shrinkWidth;
        objTop += shrinkHeight;
      }

      if (player.PosX + player.Width < objLeft) return false;
      if (player.PosX > objRight) return false;
      if (player.PosY + player.Height < objTop) return false;
      if (player.PosY > objBottom) return false;

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
