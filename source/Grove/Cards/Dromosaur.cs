namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Modifiers;

  public class Dromosaur : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Dromosaur")
        .ManaCost("{2}{R}")
        .Type("Creature Lizard")
        .Text("Whenever Dromosaur blocks or becomes blocked, it gets +2/-2 until end of turn.")
        .FlavorText(
          "They say dromosaurs are frightened of dogs, even little ones. There are no dogs in Shiv. Not even little ones.")
        .Power(2)
        .Toughness(3)
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever Dromosaur blocks or becomes blocked, it gets +2/-2 until end of turn.";
            p.Trigger(new OnBlock(becomesBlocked: true, blocks: true));
            p.Effect = () => new ApplyModifiersToSelf(
              () => new AddPowerAndToughness(2, -2) {UntilEot = true}) {ToughnessReduction = 2};

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}