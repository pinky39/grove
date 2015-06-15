namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.RepetitionRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class SoulOfZendikar : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Soul of Zendikar")
        .ManaCost("{4}{G}{G}")
        .Type("Creature — Avatar")
        .Text("{Reach}{EOL}{3}{G}{G}: Put a 3/3 green Beast creature token onto the battlefield.{EOL}{3}{G}{G}, Exile Soul of Zendikar from your graveyard: Put a 3/3 green Beast creature token onto the battlefield.")
        .Power(6)
        .Toughness(6)
        .SimpleAbilities(Static.Reach)
        .ActivatedAbility(p =>
        {
          p.Text = "{3}{G}{G}: Put a 3/3 green Beast creature token onto the battlefield.";
          p.Cost = new PayMana("{3}{G}{G}".Parse(), supportsRepetitions: true);

          p.Effect = () => new CreateTokens(
              count: 1,
              token: Card
                .Named("Beast")
                .Power(3)
                .Toughness(3)
                .Type("Token Creature - Beast")
                .Colors(CardColor.Green));

          p.TimingRule(new Any(
              new AfterOpponentDeclaresAttackers(),
              new WhenOwningCardWillBeDestroyed(),
              new OnEndOfOpponentsTurn()));

          p.RepetitionRule(new RepeatMaxTimes());
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{3}{G}{G}, Exile Soul of Zendikar from your graveyard: Put a 3/3 green Beast creature token onto the battlefield.";
          p.Cost = new AggregateCost(
            new PayMana("{3}{G}{G}".Parse()),
            new Exile(fromGraveyard: true));

          p.Effect = () => new CreateTokens(
              count: 1,
              token: Card
                .Named("Beast")
                .Power(3)
                .Toughness(3)
                .Type("Token Creature - Beast")
                .Colors(CardColor.Green));

          p.TimingRule(new Any(
              new AfterOpponentDeclaresAttackers(),
              new WhenOwningCardWillBeDestroyed(),
              new OnEndOfOpponentsTurn()));
        });
    }
  }
}
