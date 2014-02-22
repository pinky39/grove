namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI.RepetitionRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;

  public class Morphling : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Morphling")
        .ManaCost("{3}{U}{U}")
        .Type("Creature Shapeshifter")
        .Text(
          "{U}: Untap Morphling.{EOL}{U}: Morphling gains flying until end of turn.{EOL}{U}: Morphling gains shroud until end of turn.{EOL}{1}: Morphling gets +1/-1 until end of turn.{EOL}{1}: Morphling gets -1/+1 until end of turn.")
        .Power(3)
        .Toughness(3)
        .ActivatedAbility(p =>
          {
            p.Text = "{U}: Untap Morphling.";
            p.Cost = new PayMana(Mana.Blue, ManaUsage.Abilities);
            p.Effect = () => new UntapOwner();

            p.TimingRule(new OnSecondMain());
            p.TimingRule(new WhenCardHas(c => c.IsTapped));
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{U}: Morphling gains flying until end of turn.";
            p.Cost = new PayMana(Mana.Blue, ManaUsage.Abilities);
            p.Effect = () => new ApplyModifiersToSelf(
              () => new AddStaticAbility(Static.Flying) {UntilEot = true});

            p.TimingRule(new Any(
              new BeforeYouDeclareAttackers(),
              new AfterOpponentDeclaresAttackers()));

            p.TimingRule(new WhenCardHas(c => !c.Has().Flying));
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{U}: Morphling gains shroud until end of turn.";
            p.Cost = new PayMana(Mana.Blue, ManaUsage.Abilities);
            p.Effect = () => new ApplyModifiersToSelf(
              () => new AddStaticAbility(Static.Shroud) {UntilEot = true});

            p.TimingRule(new WhenCardHas(c => !c.Has().Shroud));
            p.TimingRule(new WhenOwningCardWillBeDestroyed(targetOnly: true, considerCombat: false));
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{1}: Morphling gets +1/-1 until end of turn.";
            p.Cost = new PayMana(1.Colorless(), ManaUsage.Abilities, supportsRepetitions: true);
            p.Effect = () => new ApplyModifiersToSelf(() => new AddPowerAndToughness(1, -1) {UntilEot = true});
            p.TimingRule(new WhenCardHas(c => c.Toughness > 1 && c.Toughness <= 3));
            p.TimingRule(new PumpOwningCardTimingRule(1, -1));
            p.RepetitionRule(new RepeatMaxTimes(2));
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{1}: Morphling gets -1/+1 until end of turn.";
            p.Cost = new PayMana(1.Colorless(), ManaUsage.Abilities, supportsRepetitions: true);
            p.Effect = () => new ApplyModifiersToSelf(() => new AddPowerAndToughness(-1, 1) {UntilEot = true});
            p.TimingRule(new WhenCardHas(c => c.Toughness >= 3));
            p.TimingRule(new PumpOwningCardTimingRule(-1, 1));
            p.RepetitionRule(new RepeatMaxTimes(4));
          });
    }
  }
}