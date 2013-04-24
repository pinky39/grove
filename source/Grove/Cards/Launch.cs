namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Modifiers;
  using Gameplay.Zones;

  public class Launch : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Launch")
        .ManaCost("{1}{U}")
        .Type("Enchantment - Aura")
        .Text(
          "Enchanted creature has flying.{EOL}When Launch is put into a graveyard from the battlefield, return Launch to its owner's hand.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() => new AddStaticAbility(Static.Flying));
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new FirstMain());
            p.TargetingRule(new CombatEnchantment());
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When Launch is put into a graveyard from the battlefield, return Launch to its owner's hand.";
            p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.Graveyard));
            p.Effect = () => new ReturnToHand(returnOwningCard: true);
          });
    }
  }
}