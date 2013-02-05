namespace Grove.Core.Effects
{
  using System.Linq;
  using Targeting;

  public class TargetPlayerGainsLifeEqualToCreatureCount : Effect
  {
    private readonly int _multiplier;

    public TargetPlayerGainsLifeEqualToCreatureCount(int multiplier = 1)
    {
      _multiplier = multiplier;
    }

    protected override void ResolveEffect()
    {
      Target.Player().Life += Players.Permanents().Count(x => x.Is().Creature)*_multiplier;
    }
  }
}