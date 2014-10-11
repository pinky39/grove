namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class FrenziedGoblin : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Frenzied Goblin")
          .ManaCost("{R}")
          .Type("Creature — Goblin Berserker")
          .Text("Whenever Frenzied Goblin attacks, you may pay {R}. If you do, target creature can't block this turn.")
          .FlavorText("What he lacks in stature, he makes up for with enthusiasm.")
          .Power(1)
          .Toughness(1)
          .TriggeredAbility(p =>
          {
            p.Text = "Whenever Frenzied Goblin attacks, you may pay {R}. If you do, target creature can't block this turn.";
            p.Trigger(new OnAttack());
            p.Effect = () => new PayManaApplyToCard(Mana.Red, () => new AddStaticAbility(Static.CannotBlock){UntilEot = true});
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new EffectTapCreature());
          });
    }
  }
}
