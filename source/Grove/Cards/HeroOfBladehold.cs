namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.CardDsl;
  using Core.Effects;
  using Core.Modifiers;
  using Core.Triggers;

  public class HeroOfBladehold : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Hero of Bladehold")
        .ManaCost("{2}{W}{W}")
        .Type("Creature Human Knight")
        .Text(
          "{Battle cry}(Whenever this creature attacks, each other attacking creature gets +1/+0 until end of turn.){EOL}Whenever Hero of Bladehold attacks, put two 1/1 white Soldier creature tokens onto the battlefield tapped and attacking.")
        .Power(3)
        .Toughness(4)
        .Abilities(
          C.TriggeredAbility(
            "Whenever this creature attacks, each other attacking creature gets +1/+0 until end of turn.",
            C.Trigger<OnAttack>(),
            C.Effect<ApplyModifiersToCreatures>((e, c) =>
              {
                e.Modifiers(c.Modifier<AddPowerAndToughness>((m, _) =>
                  m.Power = 1, untilEndOfTurn: true));

                e.Filter = (source, card) => source.OwningCard != card && card.IsAttacker;
              })),
          C.TriggeredAbility(
            "{Battle cry}Whenever Hero of Bladehold attacks, put two 1/1 white Soldier creature tokens onto the battlefield tapped and attacking.",
            C.Trigger<OnAttack>(),
            C.Effect<CreateTokens>((e, c) =>
              {
                e.Tokens(c.Card
                  .Named("Soldier Token")
                  .FlavorText(
                    "If you need an example to lead others to the front lines, consider the precedent set.")
                  .Power(1)
                  .Toughness(1)
                  .Type("Creature Token Soldier")
                  .Colors(ManaColors.White));

                e.Count = 2;

                e.AfterTokenComesToPlay = (token, game) => { game.Combat.JoinAttack(token); };
              })
            )
        );
    }
  }
}