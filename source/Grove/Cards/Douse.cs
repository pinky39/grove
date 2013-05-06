namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Characteristics;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;

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
            p.Cost = new PayMana("{1}{U}".Parse(), ManaUsage.Abilities);
            p.Effect = () => new CounterTargetSpell();
            p.TargetSelector.AddEffect(trg => trg
              .Is.CounterableSpell(c => c.HasColor(CardColor.Red))
              .On.Stack());

            p.TargetingRule(new Artifical.TargetingRules.Counterspell());
            p.TimingRule(new Artifical.TimingRules.Counterspell());
          }
        );
    }
  }
}