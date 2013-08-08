namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Characteristics;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.Triggers;

  public class HeroOfBladehold : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hero of Bladehold")
        .ManaCost("{2}{W}{W}")
        .Type("Creature Human Knight")
        .Text(
          "{Battle cry}(Whenever this creature attacks, each other attacking creature gets +1/+0 until end of turn.){EOL}Whenever Hero of Bladehold attacks, put two 1/1 white Soldier creature tokens onto the battlefield tapped and attacking.")
        .Power(3)
        .Toughness(4)
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever this creature attacks, each other attacking creature gets +1/+0 until end of turn.";
            p.Trigger(new OnAttack());
            p.Effect = () => new ApplyModifiersToPermanents(
              permanentFilter: (effect, card) => effect.Source.OwningCard != card && card.IsAttacker,
              modifiers: () => new AddPowerAndToughness(1, 0) {UntilEot = true});
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever Hero of Bladehold attacks, put two 1/1 white Soldier creature tokens onto the battlefield tapped and attacking.";

            p.Trigger(new OnAttack());
            p.Effect = () => new CreateTokens(
              count: 2,
              token: Card
                .Named("Soldier Token")
                .FlavorText(
                  "If you need an example to lead others to the front lines, consider the precedent set.")
                .Power(1)
                .Toughness(1)
                .Type("Creature Token Soldier")
                .Colors(CardColor.White),
              afterTokenComesToPlay: (token, game) => game.Combat.JoinAttack(token));
          });
    }
  }
}