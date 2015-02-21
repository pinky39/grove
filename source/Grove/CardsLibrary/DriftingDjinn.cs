namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Triggers;

  public class DriftingDjinn : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, sacrifice Drifting Djinn unless you pay {1}{U}.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect = () => new PayManaThen("{1}{U}".Parse(),
              effect: new SacrificeOwner(),
              parameters: new PayThen.Parameters()
              {
                ExecuteIfPaid = false,
                Message = "Pay upkeep?"
              });
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}