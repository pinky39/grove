namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Effects;
  using Core.Modifiers;

  public class GraspOfDarkness : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Grasp of Darkness")
        .ManaCost("{B}{B}")
        .Type("Instant")
        .Text("Target creature gets -4/-4 until end of turn.")
        .FlavorText("On a world with five suns, night is compelled to become an aggressive force.")
        .Timing(Timings.InstantRemoval)
        .Category(EffectCategories.PwTReduction)
        .Effect<ApplyModifiersToTarget>((e, c) => e.Modifiers(
          c.Modifier<AddPowerAndToughness>((m, _) => {
            m.Power = -4;
            m.Toughness = -4;
          }, untilEndOfTurn: true)))
        .Target(C.Selector(
          validator: target => target.Is().Creature,
          scorer: TargetScores.OpponentStuffScoresMore(spellsDamage: 4, reducesPwt: true)));
    }
  }
}