namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Modifiers;
  using Triggers;

  public class SeekerOfTheWay : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Seeker of the Way")
        .ManaCost("{1}{W}")
        .Type("Creature — Human Warrior")
        .Text("{Prowess} {I}(Whenever you cast a noncreature spell, this creature gets +1/+1 until end of turn.){/I}{EOL}Whenever you cast a noncreature spell, Seeker of the Way gains lifelink until end of turn.")
        .FlavorText("\"I don't know where my destiny lies, but I know it isn't here.\"")
        .Power(2)
        .Toughness(2)
        .Prowess()
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever you cast a noncreature spell, Seeker of the Way gains lifelink until end of turn.";
          p.Trigger(new OnCastedSpell((c, ctx) =>
            c.Controller == ctx.You && !c.Is().Creature));

          p.Effect = () => new ApplyModifiersToSelf(() => new AddStaticAbility(Static.Lifelink) { UntilEot = true });

          p.TriggerOnlyIfOwningCardIsInPlay = true;
        });
    }
  }
}
