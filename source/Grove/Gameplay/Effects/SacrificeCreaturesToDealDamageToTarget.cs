namespace Grove.Gameplay.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Characteristics;
  using Decisions;
  using Decisions.Results;
  using Targeting;
  using Zones;

  public class SacrificeCreaturesToDealDamageToTarget : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
  {
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
      Enqueue<SelectCards>(Controller,
        p =>
          {
            p.Validator(c => c.Is().Creature);
            p.Zone = Zone.Battlefield;
            p.MinCount = 0;
            p.MaxCount = null;
            p.Text = "Select creatures to sacrifice.";
            p.OwningCard = Source.OwningCard;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
          }
        );
    }
  }
}