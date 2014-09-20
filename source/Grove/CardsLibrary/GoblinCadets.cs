namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Triggers;

  public class GoblinCadets : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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