namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class AbzanAscendancy : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Abzan Ascendancy")
        .ManaCost("{W}{B}{G}")
        .Type("Enchantment")
        .Text("When Abzan Ascendancy enters the battlefield, put a +1/+1 counter on each creature you control.{EOL}Whenever a nontoken creature you control dies, put a 1/1 white Spirit creature token with flying onto the battlefield.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
        {
          p.Text = "When Abzan Ascendancy enters the battlefield, put a +1/+1 counter on each creature you control.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
          p.Effect = () => new ApplyModifiersToPermanents(
              selector: (c, ctx) => c.Is().Creature && ctx.You == c.Controller,              
              modifier: () => new AddCounters(() => new PowerToughness(1, 1), 1));
        })
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever a nontoken creature you control dies, put a 1/1 white Spirit creature token with flying onto the battlefield.";

          p.Trigger(new OnZoneChanged(
            from: Zone.Battlefield,
            to: Zone.Graveyard,
            selector: (c, ctx) => ctx.You == c.Controller && c.Is().Creature && !c.Is().Token));

          p.Effect = () => new CreateTokens(
              count: 1,
              token: Card
                .Named("Spirit")
                .Power(1)
                .Toughness(1)
                .Type("Token Creature - Spirit")
                .Text("{Flying}")
                .Colors(CardColor.White)
                .SimpleAbilities(Static.Flying));
        });
    }
  }
}
