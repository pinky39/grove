namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;

  public class GoblinRaider : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Goblin Raider")
        .ManaCost("{1}{R}")
        .Type("Creature Goblin Warrior")
        .Text("Goblin Raider can't block.")
        .FlavorText("He was proud to wear the lizard skin around his waist, just for the fun of annoying the enemy.")
        .Power(2)
        .Toughness(2)
        .StaticAbilities(Static.CannotBlock);
    }
  }
}