namespace Grove.Gameplay.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Decisions.Results;
  using Zones;

  public class EachPlayerPutsCardToBattlefield : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly Func<Card, bool> _selector;
    private readonly Zone _zone;

    private EachPlayerPutsCardToBattlefield() {}

    public EachPlayerPutsCardToBattlefield(Zone zone, Func<Card, bool> filter = null)
    {
      _zone = zone;
      _selector = filter ?? delegate { return true; };
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      var ordered = candidates
        .OrderBy(x => -x.Score)        
        .ToList();

      foreach (var card in ordered)
      {
        // not an aura just choose the card
        if (!card.Is().Aura)
          return new ChosenCards(card);

        
        // find something to attach aura to
        // or skip to next best card
        var bestAuraTarget = card.Controller.Battlefield
          .Where(target => card.CanTarget(target) && card.IsGoodTarget(target))
          .OrderBy(x => -x.Score)
          .FirstOrDefault();
        
        if (bestAuraTarget != null)
        {
          return new ChosenCards(new[] {card, bestAuraTarget});
        }
      }

      return ChosenCards.None;
    }

    public void ProcessResults(ChosenCards results)
    {
      if (results.Count == 0)
       return;

      if (results.Count == 1)
      {
        results[0].PutToBattlefield();
        return;
      }

      results[0].EnchantWithoutPayingCost(results[1]);      
    }

    private void ChooseCardToPutIntoPlay(Player player)
    {
      Enqueue<SelectCards>(
        controller: player,
        init: p =>
          {
            p.MinCount = 1;
            p.MaxCount = 1;
            p.Text = "Select a card.";
            p.Validator(_selector);
            p.Zone = _zone;
            p.OwningCard = Source.OwningCard;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
            p.AurasNeedTarget = true;
          }
        );
    }

    protected override void ResolveEffect()
    {
      ChooseCardToPutIntoPlay(Players.Active);
      ChooseCardToPutIntoPlay(Players.Passive);
    }
  }
}