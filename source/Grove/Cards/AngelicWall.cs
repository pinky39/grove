namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Dsl;

  public class AngelicWall : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Angelic Wall")
        .ManaCost("{1}{W}")
        .Type("Creature Wall")
        .Text("{Defender}, {Flying}")
        .FlavorText(
          "'The air stirred as if fanned by angels wings, and the enemy was turned aside.'{EOL}—Tales of Ikarov the Voyager")
        .Power(0)
        .Toughness(4)        
        .Abilities(
          Static.Defender,
          Static.Flying
        );
    }
  }
}