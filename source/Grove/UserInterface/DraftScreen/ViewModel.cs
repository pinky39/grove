namespace Grove.UserInterface.DraftScreen
{
  using System;
  using System.Collections.Generic;
  using Artifical;
  using Gameplay;
  using Gameplay.Tournaments;

  public class ViewModel : IDraftCardPicker
  {
    public CardInfo PickCard(List<CardInfo> draftedCards, List<CardInfo> booster, int round, CardRatings ratings)
    {
      throw new NotImplementedException();
    }

    public interface IFactory
    {
      ViewModel Create(IEnumerable<TournamentPlayer> players);
    }
  }
}