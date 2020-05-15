namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Modifiers;
  using Triggers;

  public class SageOfTheInwardEye : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sage of the Inward Eye")
        .ManaCost("{2}{U}{R}{W}")
        .Type("Creature — Djinn Wizard")
        .Text("{Flying}{EOL}Whenever you cast a noncreature spell, creatures you control gain lifelink until end of turn.")
        .FlavorText("\"No one petal claims beauty for the lotus.\"")
        .Power(3)
        .Toughness(4)
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever you cast a noncreature spell, creatures you control gain lifelink until end of turn.";
          p.Trigger(new OnCastedSpell((c, ctx) =>
            c.Controller == ctx.You && !c.Is().Creature));

          p.Effect = () => new ApplyModifiersToPermanents(
            selector: (c, ctx) => c.Is().Creature && ctx.You == c.Controller,             
            modifier: () => new AddSimpleAbility(Static.Lifelink) { UntilEot = true });

          p.TriggerOnlyIfOwningCardIsInPlay = true;
        });
    }
  }
}
