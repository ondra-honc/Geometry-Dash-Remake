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

    public GameObject(float X, float Y, float SizX, float SizY, bool IsOnScr, ObjectType ObjType)
    {
      PosX = X;
      PosY = Y;
      SizeX = SizX;
      SizeY = SizY;
      IsOnScreen = IsOnScr;
      Type = ObjType;
    }
  }
}
