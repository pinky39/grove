namespace Grove.Core.Effects
{
  using System;
  using Modifiers;

  public class PutIntoPlay : Effect
  {
    public bool PutIntoPlayTapped;

    public Value ToughnessReduction = 0;
    public Func<PutIntoPlay, Card, bool> ToughnessReductionFilter = delegate { return true; };


    public override int CalculateToughnessReduction(Card card)
    {
      var reduction = ToughnessReduction.GetValue(X);

      if (reduction > 0)
        return ToughnessReductionFilter(this, card) ? reduction : 0;

      return 0;
    }

    protected override void ResolveEffect()
    {
      if (PutIntoPlayTapped)
      {
        Source.OwningCard.Tap();
      }
    }
  }
}