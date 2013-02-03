namespace Grove.Core.Effects
{
  using System;
  using Modifiers;

  public class PutIntoPlay : Effect
  {
    private readonly bool _putIntoPlayTapped;
    private readonly Value _toughnessReduction;
    private readonly Func<PutIntoPlay, Card, bool> _toughnessReductionFilter;
    
    private PutIntoPlay() {}

    public PutIntoPlay(bool putIntoPlayTapped = false, Value toughnessReduction = null,
      Func<PutIntoPlay, Card, bool> toughnessReductionFilter = null)
    {
      _putIntoPlayTapped = putIntoPlayTapped;
      _toughnessReduction = toughnessReduction ?? 0;
      _toughnessReductionFilter = toughnessReductionFilter ?? delegate { return true; };
    }

    public override int CalculateToughnessReduction(Card card)
    {
      var reduction = _toughnessReduction.GetValue(X);

      if (reduction > 0)
        return _toughnessReductionFilter(this, card) ? reduction : 0;

      return 0;
    }

    protected override void ResolveEffect()
    {
      if (_putIntoPlayTapped)
      {
        Source.OwningCard.Tap();
      }
    }
  }
}