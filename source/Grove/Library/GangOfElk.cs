namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;
  using Grove.Gameplay.Triggers;

  public class GangOfElk : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Gang of Elk")
        .ManaCost("{5}{G}")
        .Type("Creature Elk Beast")
        .Text("Whenever Gang of Elk becomes blocked, it gets +2/+2 until end of turn for each creature blocking it.")
        .FlavorText(
          "The elk is Gaea's favorite, who wears the forest on its brow.")
        .Power(5)
        .Toughness(4)
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever Gang of Elk becomes blocked, it gets +2/+2 until end of turn for each creature blocking it.";
            p.Trigger(new OnBlock(becomesBlocked: true, triggerForEveryCreature: true));
            p.Effect = () => new ApplyModifiersToSelf(() => new AddPowerAndToughness(2, 2) {UntilEot = true});
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}