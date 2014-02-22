namespace Grove.Gameplay.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;

  public class DealDamageToCreatureWithAttributeSelectIfMoreThanOne : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly int _amount;
    private readonly Func<Game, int?> _getAttribute;
    private readonly Func<Card, int?, bool> _hasAttribute;

    private DealDamageToCreatureWithAttributeSelectIfMoreThanOne() {}

    public DealDamageToCreatureWithAttributeSelectIfMoreThanOne(int amount,
      Func<Card, int?, bool> hasAttribute, Func<Game, int?> getAttribute)
    {
      _amount = amount;
      _hasAttribute = hasAttribute;
      _getAttribute = getAttribute;
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

      Source.OwningCard.DealDamageTo(_amount, results[0], isCombat: false);
    }

    public override int CalculateCreatureDamage(Card creature)
    {
      var rank = _getAttribute(Game);
      return _hasAttribute(creature, rank) ? _amount : 0;
    }

    protected override void ResolveEffect()
    {
      var rank = _getAttribute(Game);

      Enqueue(new SelectCards(
        Controller,
        p =>
          {
            p.MinCount = 1;
            p.MaxCount = 1;
            p.Text = "Select a creature.";
            p.SetValidator(card => card.Is().Creature && _hasAttribute(card, rank));
            p.CanSelectOnlyCardsControlledByDecisionController = false;
            p.Zone = Zone.Battlefield;
            p.OwningCard = Source.OwningCard;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
          }
        ));
    }
  }
}