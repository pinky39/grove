namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using Effects;
  using Events;
  using Modifiers;
  using Triggers;

  public class NobleHierarch : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Noble Hierarch")
        .ManaCost("{G}")
        .Type("Creature - Human Druid")
        .Text("Exalted{I}(Whenever a creature you control attacks alone, that creature gets +1/+1 until end of turn.){/I}{EOL}{T}: Add {G},{W}, or {U} to your mana pool.")
        .FlavorText("She protects the sacred groves from blight, drought, and the Unbeholden.")
        .Power(0)
        .Toughness(1)
        .Exalted()
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {G}, {W} or {U} to your mana pool.";
          p.ManaAmount(Mana.Colored(isGreen: true, isBlue: true, isWhite: true));
        }); 
    }
  }
}
