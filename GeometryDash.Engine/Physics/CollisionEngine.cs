using GeometryDash.Engine.Entities;

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
  }
}
