namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Characteristics;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Mana;

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

            p.TargetingRule(new Ai.TargetingRules.Counterspell());
            p.TimingRule(new Ai.TimingRules.Counterspell());
          }
        );
    }
  }
}