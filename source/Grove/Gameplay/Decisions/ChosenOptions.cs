namespace Grove.Gameplay.Decisions
{
  using System;
  using System.Collections.Generic;

  [Serializable]
  public class ChosenOptions
  {
    private readonly List<object> _options = new List<object>();

    public ChosenOptions(params object[] options)
    {
      _options.AddRange(options);
    }

    public IList<object> Options { get { return _options; } }
  }
}