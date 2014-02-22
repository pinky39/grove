namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;

  public class HoppingAutomaton : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hopping Automaton")
        .ManaCost("{3}")
        .Type("Artifact Creature")
        .Text(
          "{0}: Hopping Automaton gets -1/-1 and gains flying until end of turn.")
        .Power(2)
        .Toughness(2)
        .ActivatedAbility(p =>
          {
            p.Text = "{0}: Hopping Automaton gets -1/-1 and gains flying until end of turn.";
            p.Cost = new PayMana(Mana.Zero, ManaUsage.Abilities);
            p.Effect = () => new ApplyModifiersToSelf(
              () => new AddPowerAndToughness(-1, -1) {UntilEot = true},
              () => new AddStaticAbility(Static.Flying) {UntilEot = true});

            p.TimingRule(new Any(new BeforeYouDeclareAttackers(), new AfterOpponentDeclaresAttackers()));
            p.TimingRule(new WhenCardHas(c => c.Toughness > 1 && !c.Has().Flying));
          });
    }
  }
}