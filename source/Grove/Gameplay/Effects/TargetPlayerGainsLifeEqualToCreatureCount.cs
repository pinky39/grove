namespace Grove.Gameplay.Effects
{
  using System;
  using System.Linq;
  using Targeting;

  [Serializable]
  public class TargetPlayerGainsLifeEqualToCreatureCount : Effect
  {
    private readonly int _multiplier;

    private TargetPlayerGainsLifeEqualToCreatureCount() {}

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