namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Mana;
  using Gameplay.Modifiers;
  using Gameplay.Zones;

  public class FieryMantle : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Fiery Mantle")
        .ManaCost("{1}{R}")
        .Type("Enchantment - Aura")
        .Text(
          "Enchant creature{EOL}{R}: Enchanted creature gets +1/+0 until end of turn.{EOL}When Fiery Mantle is put into a graveyard from the battlefield, return Fiery Mantle to its owner's hand.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() =>
              {
                var ap = new ActivatedAbilityParameters
                  {
                    Text = "{R}: Enchanted creature gets +1/+0 until end of turn.",
                    Cost = new PayMana(Mana.Red, ManaUsage.Abilities),
                    Effect = () => new ApplyModifiersToSelf(() => new AddPowerAndToughness(1, 0) {UntilEot = true})
                  };

                ap.TimingRule(new IncreaseOwnersPowerOrToughness(1, 0));

                return new AddActivatedAbility(new ActivatedAbility(ap));
              });

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TimingRule(new FirstMain());
            p.TargetingRule(new CombatEnchantment());
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Fiery Mantle is put into a graveyard from the battlefield, return Fiery Mantle to its owner's hand.";
            p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.Graveyard));
            p.Effect = () => new Gameplay.Effects.ReturnToHand(returnOwningCard: true);
          });
    }
  }
}