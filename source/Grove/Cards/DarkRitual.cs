namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Mana;

  public class DarkRitual : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Dark Ritual")
        .ManaCost("{B}")
        .Type("Instant")
        .Text("Add {B}{B}{B} to your mana pool.")
        .FlavorText(
          "'From void evolved Phyrexia. Great Yawgmoth, Father of Machines, saw its perfection. Thus The Grand Evolution began.'{EOL}—Phyrexian Scriptures")
        .OverrideScore(80) /* ritual score must be lowered a bit so ai casts it more eagerly */
        .Cast(p =>
          {
            p.Timing = Timings.HasSpellsThatNeedAdditionalMana(2);
            p.Effect = Effect<AddManaToPool>(e => e.Amount = "{B}{B}{B}".ParseMana());
          });
    }
  }
}