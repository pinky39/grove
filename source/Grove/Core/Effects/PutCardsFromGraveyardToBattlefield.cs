namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Infrastructure;

  public class PutCardsFromGraveyardToBattlefield : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly bool _eachPlayer;
    private readonly Func<Card, bool> _filter;
    private readonly Action<Card> _modify;
    private readonly TrackableList<Card> _aurasQueue = new TrackableList<Card>();

    private PutCardsFromGraveyardToBattlefield() {}

    public PutCardsFromGraveyardToBattlefield(Func<Card, bool> filter, 
      Action<Card> modify = null, bool eachPlayer = false)
    {
      _filter = filter;
      _modify = modify ?? delegate { };
      _eachPlayer = eachPlayer;
    }

    protected override void ResolveEffect()
    {
      if (_eachPlayer == false)
      {
        PutPlayersCardsToBattlefield(Controller);
        return;
      }

      PutPlayersCardsToBattlefield(Players.Active);
      PutPlayersCardsToBattlefield(Players.Passive);
    }

    protected override void Initialize()
    {
      _aurasQueue.Initialize(ChangeTracker);
    }

    private void PutPlayersCardsToBattlefield(Player player)
    {
      var cards = player.Graveyard.Where(_filter).ToList();
      var auras = cards.Where(x => x.Is().Aura);

      foreach (var card in cards.Where(x => !x.Is().Aura))
      {
        card.PutToBattlefield();
        _modify(card);
      }

      foreach (var aura in auras)
      {
        PutAuraToBattlefield(aura, player);
      }
    }

    private void PutAuraToBattlefield(Card aura, Player controller)
    {
      Enqueue(new SelectCards(
       controller,
       p =>
       {
         p.MinCount = 1;
         p.MaxCount = 1;
         p.Text = String.Format("Select a permanent to attach to.");
         p.SetValidator(aura.CanTarget);
         p.Zone = Zone.Battlefield;
         p.OwningCard = aura;
         p.ProcessDecisionResults = this;
         p.ChooseDecisionResults = this;
         p.CanSelectOnlyCardsControlledByDecisionController = false;
       }));

      _aurasQueue.Add(aura);
    }

    public void ProcessResults(ChosenCards results)
    {
      var aura = _aurasQueue.First();

      if (results.Count > 0)
      {
        var target = results[0];
        aura.EnchantWithoutPayingCost(target);
        _modify(aura);
      }

      _aurasQueue.Remove(aura);
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      var aura = _aurasQueue.First();

      var result = candidates        
        .Where(t => aura.IsGoodTarget(t, aura.Controller))        
        .OrderByDescending(x => x.Score)
        .FirstOrDefault();

      if (result == null)
      {
        result = candidates
          .OrderBy(x => x.Score)
          .FirstOrDefault();
      }

      return new ChosenCards(result);
    }
  }
}