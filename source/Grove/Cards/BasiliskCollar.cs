namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Costs;
  using Core.Effects;
  using Core.Modifiers;

  public class BasiliskCollar : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Basilisk Collar")
        .ManaCost("{1}")
        .Type("Artifact - Equipment")
        .Text("Equipped creature has deathtouch and lifelink.{EOL}{Equip} {2}")
        .FlavorText(
          "During their endless travels, the mages of the Goma Fada caravan have learned ways to harness both life and death.")
        .Timing(Timings.Steps(Step.FirstMain))
        .Abilities(
          C.ActivatedAbility(
            "{2}: Attach to target creature you control. Equip only as a sorcery.",
            C.Cost<TapOwnerPayMana>((cost, _) => cost.Amount = 2.AsColorlessMana()),
            C.Effect<AttachEquipment>((e, c) => e.Modifiers(
              c.Modifier<AddStaticAbility>((m, _) => m.StaticAbility = StaticAbility.Deathtouch),
              c.Modifier<AddStaticAbility>((m, _) => m.StaticAbility = StaticAbility.Lifelink)
              )),
            selector: C.Selector(Validator.Equipment()),
            activateAsSorcery: true));
    }
  }
}