namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class RushOfBattle : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rush of Battle")
        .ManaCost("{3}{W}")
        .Type("Sorcery")
        .Text("Creatures you control get +2/+1 until end of turn. Warrior creatures you control gain lifelink until end of turn.{I}(Damage dealt by those Warriors also causes their controller to gain that much life.){/I}")
        .FlavorText("The Mardu charge reflects the dragon's speed—and its hunger.")
        .Cast(p =>
        {
          p.Effect = () => new CompoundEffect(
              new ApplyModifiersToPermanents(
                selector: (c, ctx) => c.Is().Creature && ctx.You == c.Controller,                
                modifier: () => new AddPowerAndToughness(2, 1) { UntilEot = true })
                .SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness),
              new ApplyModifiersToPermanents(
                selector: (c, ctx) => c.Is("warrior") && ctx.You == c.Controller,                
                modifier: () => new AddSimpleAbility(Static.Lifelink) { UntilEot = true }));

          p.TimingRule(new OnFirstMain());
        });
    }
  }
}
