namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;

  public class SacrificeToDealDamageToTarget : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly Func<Card, bool> _filter;

    private SacrificeToDealDamageToTarget() {}

    public SacrificeToDealDamageToTarget(Func<Card, bool> filter)
    {
      _filter = filter;
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      var maxCount = Target.Life();

      return candidates
        .OrderBy(x => x.Score)
        .Take(maxCount)
        .ToList();
    }

    public void ProcessResults(ChosenCards results)
    {
      foreach (var chosenCard in results)
      {
        chosenCard.Sacrifice();
      }

      Source.OwningCard.DealDamageTo(results.Count, (IDamageable) Target, isCombat: false);
    }

    protected override void ResolveEffect()
    {
      Enqueue(new SelectCards(Controller,
        p =>
          {
            p.SetValidator(_filter);
            p.Zone = Zone.Battlefield;
            p.MinCount = 0;
            p.MaxCount = null;
            p.Text = "Select creatures to sacrifice.";
            p.OwningCard = Source.OwningCard;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
          }
        ));
    }
  }
}