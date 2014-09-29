namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Modifiers;

  public class Festergloom : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Festergloom")
        .ManaCost("{2}{B}")
        .Type("Sorcery")
        .Text("Nonblack creatures get -1/-1 until end of turn.")
        .FlavorText("The death of a scout can be as informative as a safe return.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToPermanents(
              selector: (e, c) => c.Is().Creature && !(c.HasColor(CardColor.Black)),
              modifiers: () => new AddPowerAndToughness(-1, -1) {UntilEot = true});
          });
    }
  }
}