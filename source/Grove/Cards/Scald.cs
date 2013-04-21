namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Effects;
  using Core.Messages;
  using Core.Triggers;

  public class Scald : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Scald")
        .ManaCost("{1}{R}")
        .Type("Enchantment")
        .Text("Whenever a player taps an Island for mana, Scald deals 1 damage to that player.")
        .FlavorText("Shiv may be surrounded by water, but the mountains go far deeper.")
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever a player taps an Island for mana, Scald deals 1 damage to that player.";
            p.Trigger(new OnPermanentGetsTapped((a, c) => c.Is("island")));

            p.Effect = () => new DealDamageToPlayer(
              amount: 1,
              player: P(e => e.TriggerMessage<PermanentGetsTapped>().Permanent.Controller));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}