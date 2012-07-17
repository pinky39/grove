namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;

  public class Songstitcher : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Songstitcher")
        .ManaCost("{W}")
        .Type("Creature Human Cleric")
        .Text(
          "{1}{W}: Prevent all combat damage that would be dealt this turn by target attacking creature with flying.")
        .FlavorText("The true names of birds are songs woven into their souls.")
        .Power(1)
        .Toughness(2)
        .Timing(Timings.FirstMain())
        .Abilities(
        //C.ActivatedAbility(
        //  "{1}{W}: Prevent all combat damage that would be dealt this turn by target attacking creature with flying.",
        //  C.Cost<TapOwnerPayMana>((cost,_) => cost.Amount = "{1}{W}".ParseManaAmount()),
        //  C.Effect()
            
        //)
        );
    }
  }
}