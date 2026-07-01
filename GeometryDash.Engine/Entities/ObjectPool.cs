namespace GeometryDash.Engine.Entities
{
  public class ObjectPool
  {
    readonly Queue<Entities.GameObject> queue = new Queue<Entities.GameObject>();

    public void InitializeQueue(int capacity)
    {
      for (int i = 0; i < capacity; i++)
      {
        Entities.GameObject obj = new Entities.GameObject();
        queue.Enqueue(obj);
      }
    }

    public Entities.GameObject Get()
    {
      return queue.Count == 0 ? new Entities.GameObject() : queue.Dequeue();
    }

    public void Return(Entities.GameObject obj)
    {
      obj.IsOnScreen = false;
      queue.Enqueue(obj);
    }
  }
}
