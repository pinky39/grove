namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Triggers;

  public class DriftingDjinn : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Drifting Djinn")
        .ManaCost("{4}{U}{U}")
        .Type("Creature - Djinn")
        .Text(
          "{Flying}{EOL}At the beg. of your upkeep, sacrifice Drifting Djinn unless you pay {1}{U}.{EOL}Cycling {2}({2}, Discard this card: Draw a card.)")
        .Power(5)
        .Toughness(5)
        .Cycling("{2}")
        .StaticAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, sacrifice Drifting Djinn unless you pay {1}{U}.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect = () => new PayManaOrSacrifice("{1}{U}".ParseMana(), message: "Pay upkeep?");
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}