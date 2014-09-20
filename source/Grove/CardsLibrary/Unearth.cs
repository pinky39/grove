namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TargetingRules;

  public class Unearth : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Unearth")
        .ManaCost("{B}")
        .Type("Sorcery")
        .Text(
          "Return target creature card with converted mana cost 3 or less from your graveyard to the battlefield.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Effect = () => new PutTargetsToBattlefield();
            p.TargetSelector.AddEffect(
              trg => trg.Is.Card(c => c.Is().Creature && c.ConvertedCost <= 3).In.YourGraveyard());
            p.TargetingRule(new EffectRankBy(c => -c.Score));
          });
    }
  }
}