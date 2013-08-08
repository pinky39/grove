namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class DisruptiveStudent : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Disruptive Student")
        .ManaCost("{2}{U}")
        .Type("Creature Human Wizard")
        .Text("{T}: Counter target spell unless its controller pays {1}.")
        .FlavorText(
          "'Teferi is a problem student. Always late for class. No appreciation for constructive use of time.")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text = "{T}: Counter target spell unless its controller pays {1}.";
            p.Cost = new Tap();
            p.Effect = () => new CounterTargetSpell(doNotCounterCost: 1);
            p.TargetSelector.AddEffect(trg => trg.Is.CounterableSpell().On.Stack());

            p.TargetingRule(new Artifical.TargetingRules.Counterspell());
            p.TimingRule(new Artifical.TimingRules.Counterspell(1));
          });
    }
  }
}