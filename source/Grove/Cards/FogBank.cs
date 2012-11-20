namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Preventions;
  using Core.Dsl;

  public class FogBank : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Fog Bank")
        .ManaCost("{1}{U}")
        .Type("Creature Wall")
        .Text("{Defender}, {Flying}{EOL}Prevent all combat damage that would be dealt to and dealt by Fog Bank.")
        .Timing(Timings.SecondMain())
        .Power(0)
        .Toughness(2)
        .Preventions(
          Prevention<PreventDealt>(), 
          Prevention<PreventReceived>(p => p.CombatOnly = true))
        .Abilities(
          Static.Defender,
          Static.Flying
        );
    }
  }
}