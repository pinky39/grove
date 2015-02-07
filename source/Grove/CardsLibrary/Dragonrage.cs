namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using AI.RepetitionRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class Dragonrage : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Dragonrage")
        .ManaCost("{2}{R}")
        .Type("Instant")
        .Text("Add {R} to your mana pool for each attacking creature you control. Until end of turn, attacking creatures you control gain \"{R}: This creature gets +1/+0 until end of turn.\"")
        .FlavorText("\"Dragons in the skies of Tarkir—for the first time, it feels like my home.\"{EOL}—Sarkhan Vol")
        .Cast(p =>
        {
          p.Effect = () => new CompoundEffect(
            new AddManaToPool(P(e =>
              Mana.Colored(ManaColor.Red, e.Controller.Battlefield.Creatures.Count(x => x.IsAttacker)))),
            new ApplyModifiersToPermanents(
              selector: (e, c) => c.Is().Creature && c.IsAttacker, 
              controlledBy: ControlledBy.SpellOwner,
              modifiers: () =>
              {
                var ap = new ActivatedAbilityParameters
                {
                  Text = "{R}: This creature gets +1/+0 until end of turn.",
                  Cost = new PayMana(Mana.Red, ManaUsage.Abilities, supportsRepetitions: true),
                  Effect = () => new ApplyModifiersToSelf(
                    () => new AddPowerAndToughness(1, 0) {UntilEot = true}).SetTags(EffectTag.IncreasePower),
                };

                ap.TimingRule(new Any(new PumpOwningCardTimingRule(1, 0), new OnEndOfOpponentsTurn()));
                ap.RepetitionRule(new RepeatMaxTimes());

                return new AddActivatedAbility(new ActivatedAbility(ap));
              }));

          p.TimingRule(new AfterYouDeclareAttackers());
        });
    }
  }
}
