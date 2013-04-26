namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Modifiers;
  using Gameplay.Zones;

  public class Rancor : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Rancor")
        .ManaCost("{G}")
        .Type("Enchantment - Aura")
        .Text(
          "Enchanted creature gets +2/+0 and has trample.{EOL}When Rancor is put into a graveyard from the battlefield, return Rancor to its owner's hand.")
        .FlavorText("Hatred outlives the hateful.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(
              () => new AddPowerAndToughness(2, 0),
              () => new AddStaticAbility(Static.Trample));

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new FirstMain());
            p.TargetingRule(new CombatEnchantment());
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When Rancor is put into a graveyard from the battlefield, return Rancor to its owner's hand.";
            p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.Graveyard));
            p.Effect = () => new ReturnToHand(returnOwningCard: true);
          });
    }
  }
}