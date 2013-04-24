namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Characteristics;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Counters;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Mana;
  using Gameplay.Modifiers;

  public class RagingRavine : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Raging Ravine")
        .Type("Land")
        .Text(
          "Raging Ravine enters the battlefield tapped.{EOL}{T}: Add {R} or {G} to your mana pool.{EOL}{2}{R}{G}: Until end of turn, Raging Ravine becomes a 3/3 red and green Elemental creature with Whenever this creature attacks, put a +1/+1 counter on it. It's still a land.")
        .Cast(p => p.Effect = () => new PutIntoPlay(tap: true))
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {R} or {G} to your mana pool.";
            p.ManaAmount(Mana.Colored(isRed: true, isGreen: true));
            p.Priority = ManaSourcePriorities.OnlyIfNecessary;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{2}{R}{G}: Until end of turn, Raging Ravine becomes a 3/3 red and green Elemental creature with Whenever this creature attacks, put a +1/+1 counter on it. It's still a land.";

            p.Cost = new PayMana("{2}{R}{G}".Parse(), ManaUsage.Abilities);

            p.Effect = () => new ApplyModifiersToSelf(
              () => new ChangeToCreature(
                power: 3,
                toughness: 3,
                colors: L(CardColor.Red, CardColor.Green),
                type: "Land Creature Elemental") {UntilEot = true},
              () =>
                {
                  var tp = new TriggeredAbilityParameters
                    {
                      Text = "Whenever this creature attacks, put a +1/+1 counter on it.",
                      Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new PowerToughness(1, 1), 1))
                    };

                  tp.Trigger(new OnAttack());
                  return new AddTriggeredAbility(new TriggeredAbility(tp)) {UntilEot = true};
                });

            p.TimingRule(new Ai.TimingRules.ChangeToCreature(minAvailableMana: 5));
          });
    }
  }
}