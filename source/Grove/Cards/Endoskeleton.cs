namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Mana;
  using Core.Dsl;
  using Core.Targeting;

  public class Endoskeleton : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Endoskeleton")
        .ManaCost("{2}")
        .Type("Artifact")
        .Text(
          "You may choose not to untap Endoskeleton during your untap step.{EOL}{2},{T}: Target creature gets +0/+3 for as long as Endoskeleton remains tapped.")
        .Timing(Timings.FirstMain())
        .MayChooseNotToUntapDuringUntap()
        .Abilities(
          C.ActivatedAbility(
            "{2},{T}: Target creature gets +0/+3 for as long as Endoskeleton remains tapped.",
            C.Cost<TapOwnerPayMana>(cost =>
              {
                cost.Amount = 2.AsColorlessMana();
                cost.TapOwner = true;
              }),
            C.Effect<Core.Details.Cards.Effects.ApplyModifiersToTarget>(p => p.Effect.Modifiers(
              p.Builder.Modifier<AddPowerAndToughness>((m, c) =>
                {
                  m.Toughness = 3;
                  m.AddLifetime(new PermanentGetsUntapedLifetime(m.Source, c.ChangeTracker));
                }))),
            effectSelector: C.Selector(Selectors.Creature()),
            targetFilter: TargetFilters.IncreasePowerAndToughness(0, 3),
            timing: Timings.NoRestrictions()));
    }
  }
}