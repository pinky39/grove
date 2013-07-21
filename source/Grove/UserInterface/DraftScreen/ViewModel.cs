namespace Grove.UserInterface.DraftScreen
{
  using System;
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.Tournaments;

  public class ViewModel
  {
    
    public interface IFactory
    {
      ViewModel Create(IEnumerable<TournamentPlayer> players);
    }

    public CardInfo DraftCard(List<CardInfo> roundBooster)
    {
      throw new NotImplementedException();
    }
  }
}