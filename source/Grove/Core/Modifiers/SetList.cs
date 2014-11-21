namespace Grove.Modifiers
{
  using System.Collections.Generic;

  public class SetList<T> : PropertyModifier<List<T>>
  {
    private readonly List<T> _list;
    private readonly int _priority;

    private SetList() {}

    public SetList(List<T> list, int priority = 1)
    {
      _list = list;
      _priority = priority;
    }

    public override int Priority
    {
      get { return _priority; }
    }

    public override List<T> Apply(List<T> before)
    {
      return _list;
    }
  }
}