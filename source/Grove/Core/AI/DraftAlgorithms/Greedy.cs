namespace Grove.AI.DraftAlgorithms
{
  using System.Collections.Generic;
  using System.Linq;

  public class Greedy : DraftingStrategy
  {
    public Greedy(CardRatings ratings) : base(ratings)
    {
    }

    protected override Card ChooseCard(List<Card> booster, int round)
    {
      if (round == 1)
      {
        if (booster.Count > 8)
        {
          return GetBestCardOfSingleColorOrColorless(booster);
        }

        if (booster.Count == 8)
        {
          ChoosePrimaryAndSecondaryColors();
        }
      }

      return PickBestPrimarySecondaryOrColorless(booster);
    }

    private void ChoosePrimaryAndSecondaryColors()
    {
      var top2 = CalculateInOutDiffs()
        .OrderByDescending(x => x.Score)
        .Take(2)
        .ToArray();

      PrimaryColor = top2[0].Color;
      SecondaryColor = top2[1].Color;
    }

    private ColorScore[] CalculateInOutDiffs()
    {
      var diff = InitializeScores();

      for (var i = 0; i < In.Length; i++)
      {
        diff[0].Score = In[0].Score - Out[0].Score;
      }

      return diff;
    }
  }
}