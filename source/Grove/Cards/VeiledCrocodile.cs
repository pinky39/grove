namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Artifical.TimingRules;
  using Gameplay.Characteristics;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Triggers;

  public class VeiledCrocodile : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Veiled Crocodile")
        .ManaCost("{2}{U}")
        .Type("Enchantment")
        .Text("When a player has no cards in hand, if Veiled Crocodile is an enchantment, Veiled Crocodile becomes a 4/4 Crocodile creature.")
        .FlavorText("Some roads are paved with bad intentions.")
        .Cast(p => p.TimingRule(new FirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "When a player has no cards in hand, if Veiled Crocodile is an enchantment, Veiled Crocodile becomes a 4/4 Crocodile creature.";
            p.Trigger(new OnEffectResolved(
              filter: (ability, game) =>
                {
                  if (ability.OwningCard.Is().Enchantment == false)
                    return false;

                  return game.Players.Any(x => x.Hand.Count == 0);
                }));

            p.Effect = () => new ApplyModifiersToSelf(() => new Gameplay.Modifiers.ChangeToCreature(
              power: 4,
              toughness: 4,
              type: "Creature Crocodile",
              colors: L(CardColor.Blue)
              ));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}