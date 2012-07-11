namespace Grove.Core.Details.Cards.Effects
{
  public class OpponentDiscardsCards : Effect
  {
    public int RandomCount { get; set; }
    public int SelectedCount { get; set; }

    protected override void ResolveEffect()
    {
      var opponent = Players.GetOpponent(Controller);

      for (var i = 0; i < RandomCount; i++)
      {
        opponent.DiscardRandomCard();
      }

      if (SelectedCount > 0)
      {
        Decisions.EnqueueDiscardCards(opponent, SelectedCount);
      }
    }
  }
}