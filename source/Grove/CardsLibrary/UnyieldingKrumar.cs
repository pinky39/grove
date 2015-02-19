namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class UnyieldingKrumar : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Unyielding Krumar")
        .ManaCost("{3}{B}")
        .Type("Creature - Orc Warrior")
        .Text("{1}{W}: Unyielding Krumar gains first strike until end of turn.")
        .FlavorText("\"The man whom I call father killed the orc who sired me, offering his world and his blade in return.\"")
        .Power(3)
        .Toughness(3)
        .ActivatedAbility(p =>
        {
          p.Text = "{1}{W}: Unyielding Krumar gains first strike until end of turn.";

          p.Cost = new PayMana("{1}{W}".Parse());

          p.Effect = () => new ApplyModifiersToSelf(
            () => new AddStaticAbility(Static.FirstStrike) { UntilEot = true });

          p.TimingRule(new Any(new BeforeYouDeclareAttackers(), new AfterOpponentDeclaresAttackers()));
          p.TimingRule(new WhenCardHas(c => !c.Has().FirstStrike && !c.IsTapped));
        });
    }
  }
}
