namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class WarNameAspirant : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("War-Name Aspirant")
        .ManaCost("{1}{R}")
        .Type("Creature — Human Warrior")
        .Text("{I}Raid{/I} — War-Name Aspirant enters the battlefield with a +1/+1 counter on it if you attacked with a creature this turn.{EOL}War-Name Aspirant can't be blocked by creatures with power 1 or less.")
        .FlavorText("No battle means more to a Mardu warrior than the one that earns her war name.")
        .Power(2)
        .Toughness(1)
        .Raid()
        .MinBlockerPower(2);
    }
  }
}
