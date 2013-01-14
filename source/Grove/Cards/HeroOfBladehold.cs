namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Mana;
  using Core.Modifiers;
  using Core.Triggers;

  public class HeroOfBladehold : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Hero of Bladehold")
        .ManaCost("{2}{W}{W}")
        .Type("Creature Human Knight")
        .Text(
          "{Battle cry}(Whenever this creature attacks, each other attacking creature gets +1/+0 until end of turn.){EOL}Whenever Hero of Bladehold attacks, put two 1/1 white Soldier creature tokens onto the battlefield tapped and attacking.")
        .Power(3)
        .Toughness(4)
        .Abilities(
          TriggeredAbility(
            "Whenever this creature attacks, each other attacking creature gets +1/+0 until end of turn.",
            Trigger<OnAttack>(),
            Effect<Core.Effects.ApplyModifiersToPermanents>(p =>
              {
                p.Effect.Modifiers(Modifier<AddPowerAndToughness>(m =>
                  m.Power = 1, untilEndOfTurn: true));

                p.Effect.Filter = (effect, card) => effect.Source.OwningCard != card && card.IsAttacker;
              })),
          TriggeredAbility(
            "{Battle cry}Whenever Hero of Bladehold attacks, put two 1/1 white Soldier creature tokens onto the battlefield tapped and attacking.",
            Trigger<OnAttack>(),
            Effect<Core.Effects.CreateTokens>(p =>
              {
                p.Effect.Tokens(Card
                  .Named("Soldier Token")
                  .FlavorText(
                    "If you need an example to lead others to the front lines, consider the precedent set.")
                  .Power(1)
                  .Toughness(1)
                  .Type("Creature Token Soldier")
                  .Colors(ManaColors.White));

                p.Effect.Count = 2;

                p.Effect.AfterTokenComesToPlay = (token, game) => { game.Combat.JoinAttack(token); };
              })
            )
        );
    }
  }
}