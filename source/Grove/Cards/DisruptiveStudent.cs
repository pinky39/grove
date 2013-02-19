namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;

  public class DisruptiveStudent : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Disruptive Student")
        .ManaCost("{2}{U}")
        .Type("Creature Human Wizard")
        .Text("{T}: Counter target spell unless its controller pays {1}.")
        .FlavorText(
          "'Teferi is a problem student. Always late for class. No appreciation for constructive use of time.'{EOL}—Barrin, progress report")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text = "{T}: Counter target spell unless its controller pays {1}.";
            p.Cost = new Tap();
            p.Effect = () => new CounterTargetSpell(doNotCounterCost: 1.Colorless());
            p.TargetSelector.AddEffect(trg => trg.Is.CounterableSpell().On.Stack());

            p.TargetingRule(new Core.Ai.TargetingRules.Counterspell());
            p.TimingRule(new Core.Ai.TimingRules.Counterspell(1));
          });
    }
  }
}