namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;

  public class StudentOfWarfare : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      var card = C.Card
        .Named("Student of Warfare")
        .ManaCost("{W}")
        .Type("Creature Human Knight")
        .Text(
          "{Level up} {W}{EOL}Level 2-6: Student of Warfare has {First strike} and becomes a 3/3 creature.{EOL}Level 7+: Student of Warfare has {Double strike} and becomes a 4/4 creature.")
        .Power(1)
        .Toughness(1);

     yield return Leveler(
        card, 
        ManaAmount.White,
        Level(min: 2, max: 6, power: 3, toughness: 3, ability: StaticAbility.FirstStrike),
        Level(min: 7, power: 4, toughness: 4, ability: StaticAbility.DoubleStrike));
      
    }
  }
}