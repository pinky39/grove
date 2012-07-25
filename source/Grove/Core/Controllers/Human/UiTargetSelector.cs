namespace Grove.Core.Controllers.Human
{
  using System;
  using Targeting;

  public class UiTargetSelector : ITargetSelector
  {
    private readonly Func<Card, bool> _isValid;
    private readonly int? _maxTargetCount;
    private readonly int _minTargetCount;
    private readonly string _text;

    public UiTargetSelector(int minTargetCount, int? maxTargetCount, string text, Func<Card, bool> isValid)
    {
      _minTargetCount = minTargetCount;
      _maxTargetCount = maxTargetCount;
      _text = text;
      _isValid = isValid;
    }


    public int? MaxCount { get { return _maxTargetCount; } }

    public int MinCount { get { return _minTargetCount; } }

    public string Text { get { return _text; } }

    public bool IsValid(Target target)
    {
      if (!target.IsCard())
        return false;
      
      return _isValid(target.Card());
    }
  }
}