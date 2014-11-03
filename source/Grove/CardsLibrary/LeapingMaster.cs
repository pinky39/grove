namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class LeapingMaster : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Leaping Master")
        .ManaCost("{1}{R}")
        .Type("Creature — Human Monk")
        .Text("{2}{W}: Leaping Master gains flying until end of turn.")
        .FlavorText("\"Strength batters down barriers. Discipline ignores them.\"")
        .Power(2)
        .Toughness(1)
        .ActivatedAbility(p =>
        {
          p.Text = "{2}{W}: Leaping Master gains flying until end of turn.";
          p.Cost = new PayMana("{2}{W}".Parse(), ManaUsage.Abilities);
          p.Effect = () => new ApplyModifiersToSelf(() => new AddStaticAbility(Static.Flying){ UntilEot = true });
          p.TimingRule(new WhenCardHas(c => !c.Has().Flying));
        });
    }
  }
}
