namespace Grove.Effects
{
  using System.Linq;

  public class RangeEffectChoice : IEffectChoice
  {
    public RangeEffectChoice(int minValue, int maxValue)
    {
      Options = Enumerable.Range(minValue, maxValue)
        .Select(x => (object) x)
        .ToArray();
    }

    public object[] Options { get; private set; }
  }
}