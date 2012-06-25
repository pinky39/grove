namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Costs;
  using Core.Effects;
  using Core.Modifiers;

  public class AngelicPage : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Angelic Page")
        .ManaCost("{1}{W}")
        .Type("Creature Angel Spirit")
        .Text("{Flying}{EOL}{T}: Target attacking or blocking creature gets +1/+1 until end of turn.")
        .FlavorText("If only every message were as perfect as its bearers.")
        .Abilities(
          StaticAbility.Flying,
          C.ActivatedAbility(
            "{T}: Target attacking or blocking creature gets +1/+1 until end of turn.",
            C.Cost<TapOwnerPayMana>((cost, _) => cost.TapOwner = true),
            C.Effect<ApplyModifiersToTarget>((e, c) => e.Modifiers(
              c.Modifier<AddPowerAndToughness>((m, _) =>
                {
                  m.Power = 1;
                  m.Toughness = 1;
                },                
                untilEndOfTurn: true))),
            C.Selector((target) => target.Is().Creature && (target.Card().IsAttacker || target.Card().IsBlocker)),                
            timing: Timings.Combat()));                      
    }
  }
}