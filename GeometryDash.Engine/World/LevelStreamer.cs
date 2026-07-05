using GeometryDash.Engine.Entities;

namespace GeometryDash.Engine.World
{
  public class LevelStreamer
  {
    private readonly List<Entities.GameObject> activeObjects = new List<Entities.GameObject>();
    private int nextBlueprintIndex = 0;
    private Entities.ObjectPool pool;

    public IReadOnlyList<Entities.GameObject> ActiveObjects => activeObjects;

    public void Initialize(Entities.ObjectPool pol)
    {
      pool = pol;
      nextBlueprintIndex = 0;
      activeObjects.Clear();
    }

    public void UpdateStreaming(IReadOnlyList<LevelData> blueprints, float cameraX, float screenWidth)
    {
      while (nextBlueprintIndex < blueprints.Count && blueprints[nextBlueprintIndex].X < cameraX + screenWidth + 200)
      {
        LevelData data = blueprints[nextBlueprintIndex];
        Entities.GameObject obj = pool.Get();

        obj.PosX = data.X;
        obj.PosY = data.Y;
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
  }
}
