namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;

  public class Douse : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Douse")
        .ManaCost("{2}{U}")
        .Type("Enchantment")
        .Text("{1}{U}: Counter target red spell.")
        .FlavorText(
          "The academy's libraries were protected by fire-prevention spells. Even after the disaster, the books were intact—though forever sealed in time.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .ActivatedAbility(p =>
          {
            p.Text = "{1}{U}: Counter target red spell.";
            p.Cost = new PayMana("{1}{U}".Parse(), ManaUsage.Abilities);
            p.Effect = () => new CounterTargetSpell();
            p.TargetSelector.AddEffect(trg => trg
              .Is.CounterableSpell(e => e.HasColor(CardColor.Red))
              .On.Stack());

            p.TargetingRule(new EffectCounterspell());
            p.TimingRule(new WhenTopSpellIsCounterable());
          }
        );
    }
  }
}