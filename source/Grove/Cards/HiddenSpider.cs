namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Triggers;

  public class HiddenSpider : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Hidden Spider")
        .ManaCost("{G}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts a creature spell with flying, if Hidden Spider is an enchantment, Hidden Spider becomes a 3/5 Spider creature with reach.")
        .FlavorText("It wants only to dress you in silk.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts a creature spell with flying, if Hidden Spider is an enchantment, Hidden Spider becomes a 3/5 Spider creature with reach.";

            p.Trigger(new OnCastedSpell(
              filter: (ability, card) =>
                ability.OwningCard.Controller != card.Controller && ability.OwningCard.Is().Enchantment &&
                  card.Is().Creature &&
                    card.Has().Flying));

            p.Effect = () => new ApplyModifiersToSelf(() => new Core.Modifiers.ChangeToCreature(
              power: 3,
              toughness: 5,
              type: "Creature Spider",
              colors: ManaColors.Green));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}