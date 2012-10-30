namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class EliteArchers : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Elite Archers")
        .ManaCost("{5}{W}")
        .Type("Creature Human Soldier Archer")
        .Text("{T}: Elite Archers deals 3 damage to target attacking or blocking creature.")
        .FlavorText("Arrows fletched with the feathers of angels seldom miss their mark.")
        .Power(3)
        .Toughness(3)
        .Timing(Timings.Creatures())
        .Abilities(
          C.ActivatedAbility(
            "{T}: Elite Archers deals 3 damage to target attacking or blocking creature.",
            C.Cost<TapOwnerPayMana>(cost => cost.TapOwner = true),
            C.Effect<DealDamageToTargets>(e => e.Amount = 3),
            C.Validator(Validators.AttackerOrBlocker()),
            selectorAi: TargetSelectorAi.DealDamageSingleSelector(3),
            timing: Timings.DeclareBlockers()
            )
        );
    }
  }
}