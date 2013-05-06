namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Characteristics;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class DarkRitual : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Dark Ritual")
        .ManaCost("{B}")
        .Type("Instant")
        .Text("Add {B}{B}{B} to your mana pool.")
        .FlavorText(
          "From void evolved Phyrexia. Great Yawgmoth, Father of Machines, saw its perfection. Thus The Grand Evolution began.")
        .OverrideScore(new ScoreOverride {Hand = 80})
        /* ritual score must be lowered a bit so ai casts it more eagerly */
        .Cast(p =>
          {
            p.TimingRule(new ControllerNeedsAdditionalMana(2));
            p.Effect = () => new AddManaToPool("{B}{B}{B}".Parse());
          });
    }
  }
}