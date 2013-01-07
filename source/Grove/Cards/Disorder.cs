namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Mana;

  public class Disorder : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Disorder")
        .ManaCost("{1}{R}")
        .Type("Sorcery")
        .Text("Disorder deals 2 damage to each white creature and each player who controls a white creature.")
        .FlavorText("'Then, just when the other guys were winnin', the sky threw up.'{EOL}—Jula, goblin raider")
        .Cast(p =>
          {
            p.Timing = All(
              Timings.MainPhases(),
              Timings.OpponentHasPermanent(x => x.Is().Creature && x.HasColors(ManaColors.White)));
            p.Effect = Effect<DealDamageToEach>(e =>
              {
                e.AmountPlayer = 2;
                e.AmountCreature = 2;
                e.FilterPlayer = (self, player) => player.Battlefield.Any(
                  card => card.Is().Creature && card.HasColors(ManaColors.White));
                e.FilterCreature = (self, creature) => creature.HasColors(ManaColors.White);
              });
          });
    }
  }
}