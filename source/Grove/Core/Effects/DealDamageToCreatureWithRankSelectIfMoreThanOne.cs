namespace Grove.Core.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Decisions.Results;
  using Zones;

  public class DealDamageToCreatureWithRankSelectIfMoreThanOne : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly int _amount;
    private readonly Func<Game, int?> _calculateRank;
    private readonly Func<Card, int?, bool> _hasRank;

    private DealDamageToCreatureWithRankSelectIfMoreThanOne() {}

    public DealDamageToCreatureWithRankSelectIfMoreThanOne(int amount,
      Func<Card, int?, bool> hasRank, Func<Game, int?> calculateRank)
    {
      _amount = amount;
      _hasRank = hasRank;
      _calculateRank = calculateRank;
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      var opponents = candidates
        .Where(x => x.Controller != Controller)
        .OrderBy(x => x.Life <= _amount ? 0 : 1)
        .ThenBy(x => -x.Score)
        .Take(1)
        .ToList();

      if (opponents.Count > 0)
        return opponents;

      return candidates
        .OrderBy(x => x.Life > _amount ? 0 : 1)
        .ThenBy(x => x.Score)
        .Take(1)
        .ToList();
    }

    public void ProcessResults(ChosenCards results)
    {
      if (results.Count == 0)
        return;

      var damage = new Damage(
        Source.OwningCard,
        _amount,
        isCombat: false,
        changeTracker: ChangeTracker);

      results[0].DealDamage(damage);
    }

    public override int CalculateCreatureDamage(Card creature)
    {
      var rank = _calculateRank(Game);
      return _hasRank(creature, rank) ? _amount : 0;
    }

    protected override void ResolveEffect()
    {
      var rank = _calculateRank(Game);

      Enqueue<SelectCards>(
        controller: Controller,
        init: p =>
          {
            p.MinCount = 1;
            p.MaxCount = 1;
            p.Text = FormatText("Select a creature.");
            p.Validator = card => card.Is().Creature && _hasRank(card, rank);
            p.CanSelectOnlyCardsControlledByDecisionController = false;
            p.Zone = Zone.Battlefield;
            p.OwningCard = Source.OwningCard;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
          }
        );
    }
  }
}