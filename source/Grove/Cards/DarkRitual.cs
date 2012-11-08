namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Mana;
  using Core.Dsl;  
  
  public class DarkRitual : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Dark Ritual")
        .ManaCost("{B}")
        .Type("Instant")
        .Text("Add {B}{B}{B} to your mana pool.")
        .OverrideScore(80) /* ritual score must be lowered a bit so ai casts it more eagerly */
        .FlavorText("'From void evolved Phyrexia. Great Yawgmoth, Father of Machines, saw its perfection. Thus The Grand Evolution began.'{EOL}—Phyrexian Scriptures")
        .Effect<AddManaToPool>(e => e.Amount = "{B}{B}{B}".ParseManaAmount())
        .Timing(Timings.HasSpellsThatNeedAdditionalMana(2));
    }
  }
}