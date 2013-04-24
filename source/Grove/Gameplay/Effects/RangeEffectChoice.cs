namespace Grove.Gameplay.Effects
{
  using System.Collections.Generic;

  public class RangeEffectChoice
  {
    private readonly int _maxValue;
    private readonly int _minValue;

    public RangeEffectChoice(int minValue, int maxValue)
    {
      _minValue = minValue;
      _maxValue = maxValue;
    }

    public IEnumerable<int> Options
    {
      get
      {
        for (var i = _minValue; i <= _maxValue; i++)
        {
          yield return i;
        }
      }
    }
  }
}