namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Costs;
  using Core.Dsl;
  using Core.Mana;
  using Core.Modifiers;
  using Core.Targeting;

  public class BasiliskCollar : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Basilisk Collar")
        .ManaCost("{1}")
        .Type("Artifact - Equipment")
        .Text("Equipped creature has deathtouch and lifelink.{EOL}{Equip} {2}")
        .FlavorText(
          "During their endless travels, the mages of the Goma Fada caravan have learned ways to harness both life and death.")
        .Cast(p => p.Timing = Timings.FirstMain())
        .Abilities(
          ActivatedAbility(
            "{2}: Attach to target creature you control. Equip only as a sorcery.",
            Cost<PayMana>(cost => cost.Amount = 2.Colorless()),
            Effect<Core.Effects.Attach>(e => e.Modifiers(
              Modifier<AddStaticAbility>(m => m.StaticAbility = Static.Deathtouch),
              Modifier<AddStaticAbility>(m => m.StaticAbility = Static.Lifelink)
              )),
            effectTarget: Target(
              Validators.ValidEquipmentTarget(),
              Zones.Battlefield()),
            targetingAi: TargetingAi.CombatEquipment(),
            timing: Timings.AttachCombatEquipment(),
            activateAsSorcery: true));
    }
  }
}