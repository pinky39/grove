namespace Grove.Modifiers
{
  using System.Collections.Generic;

  public class RemoveFromList<T> : PropertyModifier<List<T>>
  {
    private readonly T _element;
    private readonly int _priority;

    private RemoveFromList() {}

    public RemoveFromList(T element, int priority = 1)
    {
      _element = element;
      _priority = priority;
    }

    public override int Priority
    {
      get { return _priority; }
    }

    public override List<T> Apply(List<T> before)
    {
      var after = new List<T>(before);
      after.Remove(_element);
      return after;
    }
  }
}