namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using Effects;
  using Events;
  using Modifiers;
  using Triggers;

  public class NobleHierarch : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Noble Hierarch")
        .ManaCost("{G}")
        .Type("Creature - Human Druid")
        .Text("Exalted{I}(Whenever a creature you control attacks alone, that creature gets +1/+1 until end of turn.){/I}{EOL}{T}: Add {G},{W}, or {U} to your mana pool.")
        .FlavorText("She protects the sacred groves from blight, drought, and the Unbeholden.")
        .Power(0)
        .Toughness(1)
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {G}, {W} or {U} to your mana pool.";
          p.ManaAmount(Mana.Colored(isGreen: true, isBlue: true, isWhite: true));
        })
        .TriggeredAbility(p =>
        {
          p.Text = "Exalted{I}(Whenever a creature you control attacks alone, that creature gets +1/+1 until end of turn.){/I}";
          p.Trigger(new AfterAttackersAreDeclared(ctx => ctx.You.IsActive && ctx.Combat.Attackers.Count() == 1));
          p.Effect = () => new ApplyModifiersToCard(P(e => e.TriggerMessage<AttackersDeclaredEvent>().Attackers.Single().Card),
            () => new AddPowerAndToughness(1, 1){UntilEot = true});
          p.TriggerOnlyIfOwningCardIsInPlay = true;
        });
    }
  }
}
