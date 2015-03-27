namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Modifiers;
  using Triggers;

  public class AnkleShanker : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Ankle Shanker")
          .ManaCost("{2}{R}{W}{B}")
          .Type("Creature — Goblin Berserker")
          .Text("{Haste}{EOL}Whenever Ankle Shanker attacks, creatures you control gain first strike and deathtouch until end of turn.")
          .FlavorText("The stature of the fighter matters less than the depth of the cut.")
          .Power(2)
          .Toughness(2)
          .SimpleAbilities(Static.Haste)
          .TriggeredAbility(p =>
          {
            p.Text = "Whenever Ankle Shanker attacks, creatures you control gain first strike and deathtouch until end of turn.";

            p.Trigger(new WhenThisAttacks());

            p.Effect = () => new ApplyModifiersToPermanents(
              selector: (c, ctx) => c.Is().Creature && ctx.You == c.Controller,               
              modifiers: new CardModifierFactory[]
              {
                () => new AddStaticAbility(Static.FirstStrike) { UntilEot = true },
                () => new AddStaticAbility(Static.Deathtouch) { UntilEot = true },
              });
          });
    }
  }
}
