namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Abilities;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.States;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class SneakAttack : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Sneak Attack")
        .ManaCost("{3}{R}")
        .Type("Enchantment")
        .Text(
          "{R}: You may put a creature card from your hand onto the battlefield. That creature gains haste. Sacrifice the creature at the beginning of the next end step.")
        .FlavorText("Nothin' beat surprise—'cept rock.")
        .Cast(p => p.TimingRule(new FirstMain()))
        .ActivatedAbility(p =>
          {
            p.Text =
              "{R}: You may put a creature card from your hand onto the battlefield. That creature gains haste. Sacrifice the creature at the beginning of the next end step.";
            p.Cost = new PayMana(Mana.Red, ManaUsage.Abilities);

            p.Effect = () => new PutSelectedCardToBattlefield(
              "Select a creature card in your hand.",
              c => c.Is().Creature,
              Zone.Hand,
              () => new AddStaticAbility(Static.Haste) {UntilEot = true},
              () =>
                {
                  var tp = new TriggeredAbilityParameters
                    {
                      Text = "Sacrifice the creature at the beginning of the next end step.",
                      Effect = () => new SacrificeOwner(),
                      TriggerOnlyIfOwningCardIsInPlay = true
                    };

                  tp.Trigger(new OnStepStart(
                    step: Step.EndOfTurn,
                    passiveTurn: true,
                    activeTurn: true));

                  tp.UsesStack = false;
                  return new AddTriggeredAbility(new TriggeredAbility(tp));
                });

            p.TimingRule(new Steps(passiveTurn: false, activeTurn: true, steps: Step.BeginningOfCombat));
            p.TimingRule(new ControllerHandCountIs(minCount: 1, selector: c => c.Is().Creature));
          });
    }
  }
}