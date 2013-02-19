namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;

  public class Douse : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Douse")
        .ManaCost("{2}{U}")
        .Type("Enchantment")
        .Text("{1}{U}: Counter target red spell.")
        .FlavorText(
          "The academy's libraries were protected by fire-prevention spells. Even after the disaster, the books were intact—though forever sealed in time.")
        .Cast(p => p.TimingRule(new FirstMain()))
        .ActivatedAbility(p =>
          {
            p.Text = "{1}{U}: Counter target red spell.";
            p.Cost = new PayMana("{1}{U}".ParseMana(), ManaUsage.Abilities);
            p.Effect = () => new CounterTargetSpell();
            p.TargetSelector.AddEffect(trg => trg
              .Is.CounterableSpell(c => c.HasColors(ManaColors.Red))
              .On.Stack());

            p.TargetingRule(new Core.Ai.TargetingRules.Counterspell());
            p.TimingRule(new Core.Ai.TimingRules.Counterspell());
          }
        );
    }
  }
}