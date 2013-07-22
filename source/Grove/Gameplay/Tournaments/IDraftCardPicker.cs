namespace Grove.Gameplay.Tournaments
{
  using System.Collections.Generic;
  using Artifical;

  public interface IDraftCardPicker
  {
    CardInfo PickCard(List<CardInfo> draftedCards, List<CardInfo> booster, int round, CardRatings ratings);
  }
}