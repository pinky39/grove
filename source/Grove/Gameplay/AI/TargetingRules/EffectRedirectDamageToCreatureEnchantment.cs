namespace Grove.Gameplay.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectRedirectDamageToCreatureEnchantment : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>()
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