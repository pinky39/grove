namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using Costs;
  using Effects;

  public class SoulOfRavnica : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Soul of Ravnica")
        .ManaCost("{4}{U}{U}")
        .Type("Creature — Avatar")
        .Text("{Flying}{EOL}{5}{U}{U}: Draw a card for each color among permanents you control.{EOL}{5}{U}{U}, Exile Soul of Ravnica from your graveyard: Draw a card for each color among permanents you control.")
        .Power(6)
        .Toughness(6)
        .SimpleAbilities(Static.Flying)
        .ActivatedAbility(p =>
        {
          p.Text = "{5}{U}{U}: Draw a card for each color among permanents you control.";
          p.Cost = new PayMana("{5}{U}{U}".Parse(), ManaUsage.Abilities);

          p.Effect = () => new DrawCards(P(e => e.Controller.Battlefield.Where(x => !x.HasColor(CardColor.None) && !x.HasColor(CardColor.Colorless)).SelectMany(x => x.Colors).Distinct().Count()));
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{5}{U}{U}, Exile Soul of Ravnica from your graveyard: Draw a card for each color among permanents you control.";
          p.Cost = new AggregateCost(
            new PayMana("{5}{U}{U}".Parse(), ManaUsage.Abilities),
            new Exile(fromGraveyard: true));

          p.Effect = () => new DrawCards(P(e => e.Controller.Battlefield.Where(x => !x.IsColorless()).Select(x => x.Colors).Distinct().Count()));
        });
    }
  }
}
