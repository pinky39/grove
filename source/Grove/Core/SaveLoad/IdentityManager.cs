namespace Grove
{
  using System.Collections.Generic;

  public class IdentityManager
  {
    private readonly Dictionary<int, object> _objects = new Dictionary<int, object>();
    private int _nextId = 1;

    public int GetId(object obj)
    {
      var id = _nextId;
      _objects.Add(id, obj);

      _nextId++;

      return id;
    }

    public object GetObject(int id)
    {
      return _objects[id];
    }
  }
}