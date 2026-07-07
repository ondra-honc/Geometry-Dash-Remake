using GeometryDash.Engine.Shared;

namespace GeometryDash.Engine.Entities
{
  public class GameObject
  { 
    public float PosX { get; set; }
    public float PosY { get; set; }
    public float SizeX { get; set; }
    public float SizeY { get; set; }
    public bool IsOnScreen { get; set; }
    public Enums.ObjectType Type { get; set; } = Enums.ObjectType.SolidBlock;

    public GameObject()
    {
      IsOnScreen = false;
    }

    public void AssignValues(float x, float y, Enums.ObjectType objType, float sizX = 50f, float sizY = 50f)
    {
      PosX = x;
      PosY = y;
      SizeX = sizX;
      SizeY = sizY;
      IsOnScreen = true;
      Type = objType;
    }
  }
}
