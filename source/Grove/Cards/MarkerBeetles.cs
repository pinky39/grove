namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.States;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class MarkerBeetles : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Marker Beetles")
        .ManaCost("{1}{G}{G}")
        .Type("Creature Insect")
        .Text("When Marker Beetles dies, target creature gets +1/+1 until end of turn.{EOL}{2}, Sacrifice Marker Beetles: Draw a card.")
        .FlavorText("In case of emergency, crush bug.")
        .Power(2)
        .Toughness(3)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Marker Beetles dies, target creature gets +1/+1 until end of turn.";

            p.Trigger(new OnZoneChanged(
              from: Zone.Battlefield,
              to: Zone.Graveyard));
         
            p.Effect = () => new ApplyModifiersToTargets(() => new AddPowerAndToughness(1, 1)
              {
                UntilEot = true
              });

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new EffectCombatEnchantment());
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{2}, Sacrifice Marker Beetles: Draw a card.";

            p.Cost = new AggregateCost(
              new PayMana(2.Colorless(), ManaUsage.Abilities),
              new Sacrifice());

            p.Effect = () => new DrawCards(1);

            p.TimingRule(new Any(
              new WhenOwningCardWillBeDestroyed(),              
              new OnStep(Step.DeclareBlockers)));

            p.TimingRule(new WhenNoOtherInstanceOfSpellIsOnStack());
          });
    }
  }
}