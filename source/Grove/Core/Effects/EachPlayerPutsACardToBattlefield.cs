namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using Decisions;
  using Infrastructure;

  public class EachPlayerPutsACardToBattlefield : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly Func<Card, bool> _selector;
    private readonly Zone _zone;
    private readonly TrackableList<Player> _playerQueue = new TrackableList<Player>();

    private EachPlayerPutsACardToBattlefield() {}

    protected override void Initialize()
    {
      _playerQueue.Initialize(ChangeTracker);
    }

    public EachPlayerPutsACardToBattlefield(Zone zone, Func<Card, bool> filter = null)
    {
      _zone = zone;
      _selector = filter ?? delegate { return true; };
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      var controller = _playerQueue.First();      
      
      return CardPicker.ChooseBestCards(
        controller: controller,
        candidates: candidates, 
        count: 1, 
        aurasNeedTarget: true);
    }

    public void ProcessResults(ChosenCards results)
    {
      if (results.Count == 1)
      {
        results[0].PutToBattlefield();        
      }
      else if (results.Count == 2)
      {
        results[0].EnchantWithoutPayingCost(results[1]);  
      }

      _playerQueue.RemoveAt(0);
    }

    private void ChooseCardToPutIntoPlay(Player player)
    {
      Enqueue(new SelectCards(
        player,
        p =>
          {
            p.MinCount = 1;
            p.MaxCount = 1;
            p.Text = "Select a card.";
            p.SetValidator(_selector);
            p.Zone = _zone;
            p.OwningCard = Source.OwningCard;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
            p.AurasNeedTarget = true;
          }
        ));

      _playerQueue.Add(player);
    }

    protected override void ResolveEffect()
    {
      ChooseCardToPutIntoPlay(Players.Active);
      ChooseCardToPutIntoPlay(Players.Passive);
    }
  }
}