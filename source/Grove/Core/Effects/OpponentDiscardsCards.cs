namespace Grove.Core.Effects
{
  using System;

  public class OpponentDiscardsCards : Effect
  {
    public int RandomCount { get; set; }
    public int SelectedCount { get; set; }
    public bool YouChooseDiscardedCards { get; set; }
    public Func<Card, bool> Filter = delegate { return true; };

    protected override void ResolveEffect()
    {      
      var opponent = Players.GetOpponent(Controller);

      if (YouChooseDiscardedCards)
      {
        opponent.RevealHand();
        
        Game.Enqueue<Decisions.DiscardCards>(
          controller: Controller,
          init: p =>
            {
              p.Count = SelectedCount;
              p.Filter = Filter;
              p.DiscardOpponentsCards = true;
            });

        return;
      }                  

      for (var i = 0; i < RandomCount; i++)
      {
        opponent.DiscardRandomCard();
      }

      if (SelectedCount == 0) 
        return;                 

      Game.Enqueue<Decisions.DiscardCards>(
        controller: opponent,
        init: p => { 
          p.Count = SelectedCount;
          p.Filter = Filter;
        });
    }
  }
}