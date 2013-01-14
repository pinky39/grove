namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Costs;
  using Core.Dsl;
  using Core.Modifiers;
  using Core.Targeting;

  public class AngelicPage : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Angelic Page")
        .ManaCost("{1}{W}")
        .Type("Creature Angel Spirit")
        .Text("{Flying}{EOL}{T}: Target attacking or blocking creature gets +1/+1 until end of turn.")
        .FlavorText("If only every message were as perfect as its bearers.")        
        .Power(1)
        .Toughness(1)
        .Abilities(
          Static.Flying,
          ActivatedAbility(
            "{T}: Target attacking or blocking creature gets +1/+1 until end of turn.",
            Cost<Tap>(),
            Effect<Core.Effects.ApplyModifiersToTargets>(e => e.Modifiers(
              Modifier<AddPowerAndToughness>(m =>
                {
                  m.Power = 1;
                  m.Toughness = 1;
                },
                untilEndOfTurn: true))),
            Target(Validators.AttackerOrBlocker(), Zones.Battlefield()),
            targetingAi: TargetingAi.PumpAttackerOrBlocker(power: 1, thougness: 1),
            timing: Timings.DeclareBlockers(),
            category: EffectCategories.ToughnessIncrease));
    }
  }
}