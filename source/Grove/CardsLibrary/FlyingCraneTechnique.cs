namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Modifiers;
  using AI.TimingRules;

  public class FlyingCraneTechnique : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Flying Crane Technique")
        .ManaCost("{3}{U}{R}{W}")
        .Type("Instant")
        .Text("Untap all creatures you control. They gain flying and double strike until end of turn.")
        .FlavorText("There are many Jeskai styles: Riverwalk imitates flowing water, Dragonfist the ancient hellkites, and Flying Crane the wild aven of the high peaks.")
        .Cast(p =>
        {
          p.Effect = () => new CompoundEffect(
            new UntapEachPermanent(filter: c => c.Is().Creature, controlledBy: ControlledBy.SpellOwner),
            new ApplyModifiersToPermanents(
              selector: (c, ctx) => c.Is().Creature && ctx.You == c.Controller,              
              modifiers: new CardModifierFactory[]
              {
                () => new AddStaticAbility(Static.Flying){UntilEot = true}, 
                () => new AddStaticAbility(Static.DoubleStrike){UntilEot = true}, 
              }));

          p.TimingRule(new Any(new BeforeYouDeclareAttackers(), new AfterOpponentDeclaresAttackers()));
        });
    }
  }
}
