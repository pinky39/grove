namespace Grove.Modifiers
{
  using System.Collections.Generic;

  public class AddToList<T> : PropertyModifier<List<T>>
  {
    private readonly List<T> _elements;
    private readonly int _priority;

    public AddToList(List<T> elements, int priority = 1)
    {
      _elements = elements;
      _priority = priority;
    }

    public AddToList(T element, int priority = 1)
    {
      _elements = new List<T> {element};
      _priority = priority;
    }

    private AddToList() {}

    public override int Priority
    {
      get { return _priority; }
    }

    public override List<T> Apply(List<T> before)
    {
      var after = new List<T>(before);
      after.AddRange(_elements);
      return after;
    }
  }
}