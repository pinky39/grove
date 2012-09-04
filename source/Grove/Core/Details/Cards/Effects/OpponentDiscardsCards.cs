namespace Grove.Core.Details.Cards.Effects
{
  public class OpponentDiscardsCards : Effect
  {
    public int RandomCount { get; set; }
    public int SelectedCount { get; set; }
    public bool YouChooseDiscardedCards { get; set; }

    protected override void ResolveEffect()
    {      
      var opponent = Players.GetOpponent(Controller);

      if (YouChooseDiscardedCards)
      {
        opponent.RevealHand();
        
        Decisions.Enqueue<Controllers.DiscardCards>(
          controller: Controller,
          init: p =>
            {
              p.Count = SelectedCount;
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

      Decisions.Enqueue<Controllers.DiscardCards>(
        controller: opponent,
        init: p => { p.Count = SelectedCount; });
    }
  }
}