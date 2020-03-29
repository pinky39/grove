namespace Grove.AI.DraftAlgorithms
{
  using System.Collections.Generic;
  using System.Linq;

  public class Forcing : DraftingStrategy
  {
    public Forcing(CardRatings ratings) : base(ratings)
    {
    }

    protected override Card ChooseCard(List<Card> booster, int round)
    {
      Card pickedCard = null;

      switch (round)
      {
        case 1:

          pickedCard = booster.Count == 15
            ? PickFirstCardOfTheDraft(booster, round)
            : BlockPrimaryOrSnatchSecondary(booster, round);

          break;

        case 3:
        case 2:
          pickedCard = PickBestPrimarySecondaryOrColorless(booster, round);
          break;
      }

      return pickedCard;
    }


    private Card BlockPrimaryOrSnatchSecondary(List<Card> booster, int round)
    {
      var outListIfPrimaryIsPicked = CloneScores(Out);
      var outListIfPrimaryIsNotPicked = CloneScores(Out);

      var primaryPick = GetBestCardOfChosenColorsOrColorless(
        color1: PrimaryColor, 
        cards: booster, 
        round: round);

      var secondaryColor = SecondaryColor.HasValue
        ? SecondaryColor.Value
        : In
          .Where(x => x.Color != PrimaryColor)
          .OrderByDescending(x => x.Score)
          .First()
          .Color;

      var secondaryPick = GetBestCardOfChosenColor(secondaryColor, booster, round);

      var primaryRating = primaryPick != null ? GetRating(primaryPick, round) : PlayableThreshold;
      var secondaryRating = secondaryPick != null ? GetRating(secondaryPick, round) : PlayableThreshold;

      if (primaryPick != null && primaryRating > PlayableThreshold)
      {
        if (secondaryPick == null)
        {
          return primaryPick;
        }

        UpdateOut(outListIfPrimaryIsPicked, booster, primaryPick, round);
        UpdateOut(outListIfPrimaryIsNotPicked, booster, secondaryPick, round);

        var notPickedRank = GetColorRank(PrimaryColor, outListIfPrimaryIsNotPicked);

        if (secondaryRating > primaryRating && notPickedRank > 2)
        {
          SecondaryColor = secondaryPick.Colors[0];
          return secondaryPick;
        }

        return primaryPick;
      }

      if (secondaryPick != null && secondaryRating > PlayableThreshold)
      {
        SecondaryColor = secondaryPick.Colors[0];
        return secondaryPick;
      }

      return PickBestColorlessOrOffcolor(booster, round);
    }

    private Card PickBestColorlessOrOffcolor(List<Card> booster, int round)
    {
      return booster
        .OrderBy(x =>
          {
            if (x.IsColorless())
              return 0;

            return 1;
          })
        .ThenByDescending(c => GetRating(c, round, usePlayerContext: false))
        .First();
    }

    private Card PickFirstCardOfTheDraft(IEnumerable<Card> booster, int round)
    {
      var card = booster
        .Where(x => !x.IsColorless())
        .OrderByDescending(c => GetRating(c, round))
        .First();

      PrimaryColor = card.Colors[0];
      return card;
    }
  }
}