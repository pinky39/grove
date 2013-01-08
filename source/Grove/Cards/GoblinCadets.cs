namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Cards.Triggers;
  using Core.Dsl;

  public class GoblinCadets : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Goblin Cadets")
        .ManaCost("{R}")
        .Type("Creature Goblin")
        .Text(
          "Whenever Goblin Cadets blocks or becomes blocked, target opponent gains control of it. (This removes Goblin Cadets from combat.)")
        .FlavorText("'If you kids don't stop that racket, I'm turning this expedition around right now!'")
        .Power(2)
        .Toughness(1)        
        .Abilities(
          TriggeredAbility(
            "Whenever Goblin Cadets blocks or becomes blocked, target opponent gains control of it. (This removes Goblin Cadets from combat.)",
            Trigger<OnBlock>(t =>
              {
                t.GetsBlocked = true;
                t.Blocks = true;
              }),
            Effect<SwitchController>(), triggerOnlyIfOwningCardIsInPlay: true));
    }
  }
}