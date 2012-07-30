namespace Grove.Core.Controllers.Human
{
  using System;
  using Targeting;

  public class UiTargetValidator : ITargetValidator
  {
    private readonly Func<Card, bool> _isValid;
    private readonly int? _maxTargetCount;
    private readonly int _minTargetCount;
    private readonly string _text;

    public UiTargetValidator(int minTargetCount, int? maxTargetCount, string text, Func<Card, bool> isValid)
    {
      _minTargetCount = minTargetCount;
      _maxTargetCount = maxTargetCount;
      _text = text;
      _isValid = isValid;
    }

    public int? MaxCount { get { return _maxTargetCount; } }
    public int MinCount { get { return _minTargetCount; } }

    public bool IsValid(ITarget target)
    {
      var card = target as Card;
      if (card == null)
        return false;

      return _isValid(card);
    }

    public string GetMessage(int targetNumber)
    {             
      var maxNumber = MinCount == MaxCount ? MaxCount.ToString() : "max. " + MaxCount.ToString();
      
      if (MaxCount > 1)
      {
        return string.Format("{0}: {1} of {2}.", _text, targetNumber, maxNumber);        
      }
      
      return _text + ".";      
    }
  }
}