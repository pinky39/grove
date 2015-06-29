namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;
  using Triggers;

  public class Goblinslide : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Goblinslide")
        .ManaCost("{2}{R}")
        .Type("Enchantment")
        .Text("Whenever you cast a noncreature spell, you may pay {1}. If you do, put a 1/1 red Goblin creature token with haste onto the battlefield.")
        .FlavorText("Goblins, like snowflakes, are only dangerous in numbers.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever you cast a noncreature spell, you may pay {1}. If you do, put a 1/1 red Goblin creature token with haste onto the battlefield.";
          p.Trigger(new OnCastedSpell((c, ctx) =>
            c.Controller == ctx.You && !c.Is().Creature));

          p.Effect = () => new PayManaThen(1.Colorless(), new CreateTokens(
            count: 1,
            token: Card
              .Named("Goblin")
              .Power(1)
              .Toughness(1)
              .Type("Token Creature - Goblin")
              .Text("{Haste}")
              .Colors(CardColor.Red)
              .SimpleAbilities(Static.Haste)));

          p.TriggerOnlyIfOwningCardIsInPlay = true;
        });
    }
  }
}
