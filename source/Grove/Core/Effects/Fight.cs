namespace Grove.Effects
{
    using System;
    using System.Linq;
  using Modifiers;

  public class Fight : Effect
  {
    private readonly Func<Card, int> _selector;

    private Fight()
    {
    }

    public Fight(Func<Card, int> selector)
    {
      _selector = selector;
    }

    protected override void ResolveEffect()
    {
      var targets = ValidEffectTargets.ToList();

      var first = targets[0].Card();
      var second = targets[1].Card();

      first.DealDamageTo(_selector(first), second, isCombat: true);
      second.DealDamageTo(_selector(second), first, isCombat: true);
    }
  }
}
