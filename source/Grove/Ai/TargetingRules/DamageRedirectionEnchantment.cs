namespace Grove.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Gameplay.Card;
  using Gameplay.Card.Characteristics;
  using Gameplay.Targeting;

  public class DamageRedirectionEnchantment : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Gameplay.Card.Card>()
        .Select(x => new
          {
            Card = x,
            Score = CalculateDamageRedirectionScore(x, p)
          })
        .Where(x => x.Score > 0)
        .OrderByDescending(x => x.Score)
        .Select(x => x.Card);

      return Group(candidates, 1);
    }

    private static int CalculateDamageRedirectionScore(Card card, TargetingRuleParameters p)
    {
      const int protectionBonus = 200;

      if (card.Controller != p.Controller)
      {
        return card.Score;
      }

      if (card.HasProtectionFrom(CardColor.Red) || card.HasProtectionFrom(CardColor.Black))
      {
        return protectionBonus + card.Score;
      }

      return card.Toughness.GetValueOrDefault() - 3;
    }
  }
}