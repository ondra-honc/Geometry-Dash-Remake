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
          float overlapLeft = (player.PosX + player.Width) - obj.PosX;
          float overlapRight = (obj.PosX + obj.SizeX) - player.PosX;
          float overlapTop = (player.PosY + player.Height) - obj.PosY;
          float overlapBottom = (obj.PosY + obj.SizeY) - player.PosY;

          float minOverlapX = Math.Min(overlapLeft, overlapRight);
          float minOverlapY = Math.Min(overlapTop, overlapBottom);

          if (minOverlapY < minOverlapX)
          {
            if (player.VelocityY >= 0 && overlapTop < overlapBottom)
            {
              player.PosY = obj.PosY - player.Height;
              player.VelocityY = 0f;
              player.IsGrounded = true;
            }
            else if (player.VelocityY < 0 && overlapBottom < overlapTop)
            {
              player.IsDead = true;
            }
          }
          else
          {
            if (overlapLeft < overlapRight)
            {
              player.IsDead = true;
            }
          }
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
