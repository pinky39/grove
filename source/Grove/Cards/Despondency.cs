namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Modifiers;
  using Gameplay.Zones;

  public class Despondency : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Despondency")
        .ManaCost("{1}{B}")
        .Type("Enchantment Aura")
        .Text(
          "Enchanted creature gets -2/-0.{EOL}When Despondency is put into a graveyard from the battlefield, return Despondency to its owner's hand.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() => new AddPowerAndToughness(-2, 0));
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new FirstMain());
            p.TargetingRule(new ReducePower());
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Despondency is put into a graveyard from the battlefield, return Despondency to its owner's hand.";
            p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.Graveyard));
            p.Effect = () => new ReturnToHand(returnOwningCard: true);
          });
    }
  }
}