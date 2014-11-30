namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;

  public class FeralIncarnation : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Feral Incarnation")
        .ManaCost("{8}{G}")
        .Type("Sorcery")
        .Text(
          "{Convoke}{I}(Your creatures can help cast this spell. Each creature you tap while casting this spell pays for {1} or one mana of that creature's color.){/I}{EOL}Put three 3/3 green Beast creature tokens onto the battlefield.")
        .FlavorText("Nature is itself wild—in all its forms.")
        .SimpleAbilities(Static.Convoke)
        .Cast(p =>
          {            
            p.Effect = () => new CreateTokens(
              count: 3,
              token: Card
                .Named("Beast")
                .Power(3)
                .Toughness(3)
                .Type("Token Creature - Beast")
                .Colors(CardColor.Green));
          });
    }
  }
}