using GeometryDash.Engine.Entities;
using GeometryDash.Engine.Core;

namespace GeometryDash.Engine.Physics
{
  public class CollisionEngine
  {
    public bool CheckOverlap(Entities.GameObject obj, Entities.PlayerCube player)
    {
      if (player.PosX + player.Width < obj.PosX) return false;

      if (player.PosX > obj.PosX + obj.SizeX) return false;

      if (player.PosY + player.Height < obj.PosY) return false;

      if (player.PosY > obj.PosY + obj.SizeY) return false;

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
