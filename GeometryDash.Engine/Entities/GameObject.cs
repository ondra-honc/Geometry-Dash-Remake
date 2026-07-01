namespace GeometryDash.Engine.Entities
{
  public class GameObject
  {
    public enum ObjectType
    {
      SolidBlock,
      Spike,
      JumpPad
    }
    
    public float PosX { get; set; }
    public float PosY { get; set; }
    public float SizeX { get; set; }
    public float SizeY { get; set; }
    public bool IsOnScreen { get; set; }
    public ObjectType Type { get; set; } = ObjectType.SolidBlock;

    public GameObject(float x, float y, float sizX, float sizY, bool isOnScr, ObjectType objType)
    {
      PosX = x;
      PosY = y;
      SizeX = sizX;
      SizeY = sizY;
      IsOnScreen = isOnScr;
      Type = objType;
    }
  }
}
