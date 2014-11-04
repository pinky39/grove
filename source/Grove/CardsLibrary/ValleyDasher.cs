namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class ValleyDasher : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Valley Dasher")
        .ManaCost("{1}{R}")
        .Type("Creature — Human Berserker")
        .Text("{Haste}{EOL}Valley Dasher attacks each turn if able.")
        .FlavorText("Mardu riders' greatest fear is that a battle might end before their weapons draw blood.")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.Haste, Static.AttacksEachTurnIfAble);
    }
  }
}
