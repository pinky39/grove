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

  public class MarshCasualties : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Marsh Casualties")
        .ManaCost("{B}{B}")
        .Type("Sorcery")
        .Text(
          "{Kicker} {3}{EOL}Creatures target player controls get -1/-1 until end of turn. If Marsh Casualties was kicked, those creatures get -2/-2 until end of turn instead.")
        .Cast(p =>
          {
            p.Effect = Effect<Core.Effects.ApplyModifiersToPermanents>(e =>
              {
                e.ToughnessReduction = 1;
                e.Filter = (effect, card) => card.Is().Creature;
                e.Modifiers(Modifier<AddPowerAndToughness>(m =>
                  {
                    m.Power = -1;
                    m.Toughness = -1;
                  }, untilEndOfTurn: true));
              });
            p.EffectTargets = L(Target(Validators.Player(), Zones.None()));
            p.TargetingAi = TargetingAi.Opponent();
          })
        .Cast(p =>
          {
            p.Text = p.KickerDescription;
            p.Cost = Cost<PayMana>(c => c.Amount = "{3}{B}{B}".ParseMana());
            p.Effect = Effect<Core.Effects.ApplyModifiersToPermanents>(e =>
              {
                e.ToughnessReduction = 2;
                e.Filter = (effect, card) => card.Is().Creature;
                e.Modifiers(Modifier<AddPowerAndToughness>(m =>
                  {
                    m.Power = -2;
                    m.Toughness = -2;
                  }, untilEndOfTurn: true));
              });
            p.EffectTargets = L(Target(Validators.Player(), Zones.None()));
            p.TargetingAi = TargetingAi.Opponent();
          });
    }
  }
}