namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.States;

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
            p.Effect = () => new PreventCombatDamage();            
            p.TimingRule(new OnOpponentsTurn(Step.DeclareBlockers));
          });
    }
  }
}