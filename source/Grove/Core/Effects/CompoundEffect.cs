namespace Grove.Core.Effects
{
  using System.Collections.Generic;
  using System.Linq;

  public class CompoundEffect : Effect
  {
    private readonly List<Effect> _childEffects = new List<Effect>();

    private CompoundEffect() {}

    public CompoundEffect(params Effect[] effects)
    {
      _childEffects.AddRange(effects);
    }

    public override int CalculateCreatureDamage(Card creature)
    {
      return _childEffects.Sum(x => x.CalculateCreatureDamage(creature));
    }

    public override int CalculatePlayerDamage(Player player)
    {
      return _childEffects.Sum(x => x.CalculatePlayerDamage(player));
    }

    public override Effect Initialize(EffectParameters p, Game game)
    {
      base.Initialize(p, game);

      var toughnessReduction = 0;

      foreach (var effect in _childEffects)
      {
        effect.Initialize(p, game);
        Category = Category | effect.Category;
        toughnessReduction = toughnessReduction + effect.ToughnessReduction.GetValue(X);
      }

      ToughnessReduction = toughnessReduction;

      return this;
    }

    protected override void ResolveEffect()
    {
      foreach (var effect in _childEffects)
      {
        effect.Resolve();
      }
    }
  }
}