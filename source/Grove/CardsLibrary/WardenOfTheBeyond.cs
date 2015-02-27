namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Modifiers;

  public class WardenOfTheBeyond : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Warden of the Beyond")
        .ManaCost("{2}{W}")
        .Type("Creature — Human Wizard")
        .Text(
          "{Vigilance} {I}(Attacking doesn't cause this creature to tap.){/I}{EOL}Warden of the Beyond gets +2/+2 as long as an opponent owns a card in exile.")
        .FlavorText("He draws strength from a vast source few mortals can fathom.")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.Vigilance)
        .StaticAbility(p =>
          {
            p.Modifier(() => new AddPowerAndToughness(2, 2));
            p.Condition = cond => cond.OpponentHasCardInExile();
          });
    }
  }
}