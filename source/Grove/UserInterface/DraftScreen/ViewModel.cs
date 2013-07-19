namespace Grove.UserInterface.DraftScreen
{
  using System.Collections.Generic;
  using Gameplay.Tournaments;

  public class ViewModel
  {
    
    public interface IFactory
    {
      ViewModel Create(IEnumerable<TournamentPlayer> players);
    }
  }
}