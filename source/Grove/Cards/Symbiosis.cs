namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class Symbiosis : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Symbiosis")
        .ManaCost("{1}{G}")
        .Type("Instant")
        .Text("Two target creatures each get +2/+2 until end of turn.")
        .FlavorText(
          "Although the elves of Argoth always considered them a nuisance, the pixies made fine allies during the war against the machines.")
        .Cast(p =>
          {
            p.Category = EffectCategories.ToughnessIncrease;
            p.Effect = Effect<ApplyModifiersToTargets>(e => e.Modifiers(
              Modifier<AddPowerAndToughness>(m =>
                {
                  m.Power = 2;
                  m.Toughness = 2;
                }, untilEndOfTurn: true)));
            p.EffectTargets =
              L(Target(Validators.Card(x => x.Is().Creature), Zones.Battlefield(), minCount: 2, maxCount: 2));
            p.TargetSelectorAi = TargetSelectorAi.IncreasePowerAndToughness(2, 2);
          });
    }
  }
}