namespace GeometryDash.Engine.Entities
{
  public class ObjectPool
  {
    readonly Queue<GameObject> queue = new Queue<GameObject>();

    public void InitializeQueue(int capacity)
    {
      for (int i = 0; i < capacity; i++)
      {
        GameObject obj = new GameObject();
        queue.Enqueue(obj);
      }
    }

    public GameObject Get()
    {
      return queue.Count == 0 ? new GameObject() : queue.Dequeue();
    }

    public void Return(GameObject obj)
    {
      obj.IsOnScreen = false;
      queue.Enqueue(obj);
    }
  }
}
