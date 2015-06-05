namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;

  public class Lull : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Lull")
        .ManaCost("{1}{G}")
        .Type("Instant")
        .Text(
          "Prevent all combat damage that would be dealt this turn.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Effect = () => new PreventAllCombatDamage();
            p.TimingRule(new AfterOpponentDeclaresAttackers());
          });
    }
  }
}