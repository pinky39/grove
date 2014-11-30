namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;

  public class TriplicateSpirits : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Triplicate Spirits")
        .ManaCost("{4}{W}{W}")
        .Type("Sorcery")
        .Text(
          "{Convoke}{I}(Your creatures can help cast this spell. Each creature you tap while casting this spell pays for {1} or one mana of that creature's color.){/I}{EOL}Put three 1/1 white Spirit creature tokens with flying onto the battlefield.{I}(They can't be blocked except by creatures with flying or reach.){/I}")
        .FlavorText("Nature is itself wild—in all its forms.")
        .SimpleAbilities(Static.Convoke)
        .Cast(p =>
          {            
            p.Effect = () => new CreateTokens(
              count: 3,
              token: Card
                .Named("Spirit")
                .Power(1)
                .Toughness(1)
                .Type("Token Creature - Spirit")
                .Text("{Flying}")
                .SimpleAbilities(Static.Flying)
                .Colors(CardColor.White));
          });
    }
  }
}