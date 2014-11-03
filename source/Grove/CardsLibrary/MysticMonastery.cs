namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;

  public class MysticMonastery : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mystic Monastery")
        .Type("Land")
        .Text("Mystic Monastery enters the battlefield tapped.{EOL}{T}: Add {U}, {R} or {W} to your mana pool.")
        .FlavorText("When asked how many paths reach enlightenment, the monk kicked a heap of sand. \"Count,\" he smiled, \"and then find more grains.\"")
        .Cast(p => p.Effect = () => new CastPermanent(tap: true))
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {U}, {R} or {W} to your mana pool.";
          p.ManaAmount(Mana.Colored(isBlue: true, isRed: true, isWhite: true));
        });
    }
  }
}
