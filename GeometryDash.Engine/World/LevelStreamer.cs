using GeometryDash.Engine.Core;
using GeometryDash.Engine.Entities;
using static GeometryDash.Engine.Shared.Enums;

namespace GeometryDash.Engine.World
{
  public class LevelStreamer
  {
    private readonly List<GameObject> activeObjects = new List<GameObject>();
    private int nextBlueprintIndex = 0;
    private ObjectPool pool;

    public IReadOnlyList<GameObject> ActiveObjects => activeObjects;

    public void Initialize(ObjectPool pol)
    {
      pool = pol;
      nextBlueprintIndex = 0;
      activeObjects.Clear();
    }

    public void UpdateStreaming(IReadOnlyList<LevelData> blueprints, float cameraX, float screenWidth)
    {
      int screenHeight = Raylib_cs.Raylib.GetScreenHeight();
      int floorY = screenHeight - (int)(screenHeight * GameSettings.floorFloat);

      int Size = 80;

      while (nextBlueprintIndex < blueprints.Count && blueprints[nextBlueprintIndex].X < cameraX + screenWidth + 200)
      {
        LevelData data = blueprints[nextBlueprintIndex];
        GameObject obj = pool.Get();

        obj.PosX = data.X;
        obj.PosY = floorY - Size - (data.Y * Size);
        obj.Type = (ObjectType)data.TypeId;

        activeObjects.Add(obj);

        nextBlueprintIndex++;
      }

      for (int i = activeObjects.Count - 1; i >= 0; i--)
      {
        GameObject obj = activeObjects[i];
        if (obj.PosX < cameraX - 100)
        {
          pool.Return(obj);
          activeObjects.RemoveAt(i);
        }
      }
    }

    public void Reset()
    {
      activeObjects.Clear();
      nextBlueprintIndex = 0;
    }
  }
}
