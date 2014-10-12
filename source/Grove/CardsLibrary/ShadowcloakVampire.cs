namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class ShadowcloakVampire : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Shadowcloak Vampire")
        .ManaCost("{4}{B}")
        .Type("Creature - Vampire")
        .Text(
          "Pay 2 life: Shadowcloak Vampire gains flying until end of turn.{I}(It can't be blocked except by creatures with flying or reach.){/I}")
        .FlavorText("\"My favorite guilty pleasure? Are there innocent ones?\"")
        .Power(4)
        .Toughness(3)
        .ActivatedAbility(p =>
          {
            p.Text =
              "Pay 2 life: Shadowcloak Vampire gains flying until end of turn. (It can't be blocked except by creatures with flying or reach.)";

            p.Cost = new PayLife(2);

            p.Effect = () => new ApplyModifiersToSelf(
              () => new AddStaticAbility(Static.Flying) {UntilEot = true});

            p.TimingRule(new Any(new BeforeYouDeclareAttackers(), new AfterOpponentDeclaresAttackers()));
            p.TimingRule(new WhenCardHas(c => !c.Has().Flying && !c.IsTapped));
          });
    }
  }
}