namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class SoulOfTheros : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Soul of Theros")
        .ManaCost("{4}{W}{W}")
        .Type("Creature — Avatar")
        .Text("{Vigilance}{EOL}{4}{W}{W}: Creatures you control get +2/+2 and gain first strike and lifelink until end of turn.{EOL}{4}{W}{W}, Exile Soul of Theros from your graveyard: Creatures you control get +2/+2 and gain first strike and lifelink until end of turn.")
        .Power(6)
        .Toughness(6)
        .SimpleAbilities(Static.Vigilance)
        .ActivatedAbility(p =>
        {
          p.Text = "{4}{W}{W}: Creatures you control get +2/+2 and gain first strike and lifelink until end of turn.";
          p.Cost = new PayMana("{4}{W}{W}".Parse(), ManaUsage.Abilities);

          p.Effect = () => new ApplyModifiersToPermanents(
            selector: (e, c) => c.Is().Creature,
            controlledBy: ControlledBy.SpellOwner,
            modifiers: new CardModifierFactory[]
            {
              () => new AddPowerAndToughness(2, 2){UntilEot = true},
              () => new AddStaticAbility(Static.FirstStrike){UntilEot = true},
              () => new AddStaticAbility(Static.Lifelink){UntilEot = true}
            }).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

          p.TimingRule(new Any(new AfterOpponentDeclaresAttackers(), new BeforeYouDeclareAttackers()));
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{4}{W}{W}, Exile Soul of Theros from your graveyard: Creatures you control get +2/+2 and gain first strike and lifelink until end of turn.";
          p.Cost = new AggregateCost(
            new PayMana("{4}{W}{W}".Parse(), ManaUsage.Abilities),
            new Exile(fromGraveyard: true));

          p.Effect = () => new ApplyModifiersToPermanents(
            selector: (e, c) => c.Is().Creature,
            controlledBy: ControlledBy.SpellOwner,
            modifiers: new CardModifierFactory[]
            {
              () => new AddPowerAndToughness(2, 2){UntilEot = true},
              () => new AddStaticAbility(Static.FirstStrike){UntilEot = true},
              () => new AddStaticAbility(Static.Lifelink){UntilEot = true}
            }).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

          p.ActivationZone = Zone.Graveyard;

          p.TimingRule(new Any(new AfterOpponentDeclaresAttackers(), new BeforeYouDeclareAttackers()));
        });
    }
  }
}
