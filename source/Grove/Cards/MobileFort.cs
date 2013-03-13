namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Modifiers;

  public class MobileFort : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Mobile Fort")
        .ManaCost("{4}")
        .Type("Artifact Creature Wall")
        .Text(
          "Defender (This creature can't attack.){EOL}{3}: Mobile Fort gets +3/-1 until end of turn and can attack this turn as though it didn't have defender. Activate this ability only once each turn.")
        .Power(0)
        .Toughness(6)
        .StaticAbilities(Static.Defender)
        .ActivatedAbility(p =>
          {
            p.Text =
              "{3}: Mobile Fort gets +3/-1 until end of turn and can attack this turn as though it didn't have defender. Activate this ability only once each turn.";
            p.Cost = new PayMana(3.Colorless(), ManaUsage.Abilities);

            p.Effect = () => new ApplyModifiersToSelf(
              () => new AddPowerAndToughness(3, -1) {UntilEot = true},
              () => new RemoveStaticAbility(Static.Defender) {UntilEot = true});

            p.ActivateOnlyOnceEachTurn = true;

            p.TimingRule(new Steps(steps: Step.BeginningOfCombat, activeTurn: true, passiveTurn: false));
          });
    }
  }
}