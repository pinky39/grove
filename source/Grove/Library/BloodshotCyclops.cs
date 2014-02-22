namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;

  public class BloodshotCyclops : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Bloodshot Cyclops")
        .ManaCost("{5}{R}")
        .Type("Creature Cyclops Giant")
        .Text(
          "{T}, Sacrifice a creature: Bloodshot Cyclops deals damage equal to the sacrificed creature's power to target creature or player.")
        .FlavorText("After their first encounter, the goblins named him Chuck.")
        .Power(4)
        .Toughness(4)
        .ActivatedAbility(p =>
          {
            p.Text =
              "{T}, Sacrifice a creature: Bloodshot Cyclops deals damage equal to the sacrificed creature's power to target creature or player.";

            p.Cost = new AggregateCost(
              new TapOwner(),
              new Sacrifice());

            p.Effect = () => new DealDamageToTargets(
              amount: P(e => e.Targets.Cost[0].Card().Power.GetValueOrDefault()));

            p.TargetSelector.AddCost(trg =>
              {
                trg.Is.Creature(ControlledBy.SpellOwner).On.Battlefield();
                trg.Message = "Select a creature to sacrifice.";
              });

            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());

            p.TargetingRule(new CostSacrificeEffectDealDamageEqualToPower());
            p.TimingRule(new OnMainStepsOfYourTurn());
          });
    }
  }
}