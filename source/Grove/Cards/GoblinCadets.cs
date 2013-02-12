namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Effects;
  using Core.Triggers;

  public class GoblinCadets : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever Goblin Cadets blocks or becomes blocked, target opponent gains control of it. (This removes Goblin Cadets from combat.)";
            p.Trigger(new OnBlock(becomesBlocked: true, blocks: true));
            p.Effect = () => new SwitchController();
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}