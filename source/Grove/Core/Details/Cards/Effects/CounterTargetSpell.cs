namespace Grove.Core.Details.Cards.Effects
{
  using Controllers;
  using Mana;
  using Targeting;

  public class CounterTargetSpell : Effect
  {
    public int? ControllersLifeloss;
    public IManaAmount DoNotCounterCost;
    public bool TapLandsEmptyPool;

    protected override void ResolveEffect()
    {
      var targetSpellController = Target().Effect().Controller;

      Game.Enqueue<PayCounterCost>(targetSpellController, d =>
        {
          d.DoNotCounterCost = DoNotCounterCost;
          d.Lifeloss = ControllersLifeloss;
          d.Spell = Target().Effect();
          d.TapLandsEmptyManaPool = TapLandsEmptyPool;
        });
      return;
    }
  }
}