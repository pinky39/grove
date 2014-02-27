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
            ? PickFirstCardOfTheDraft(booster)
            : BlockPrimaryOrSnatchSecondary(booster);

          break;

        case 3:
        case 2:
          pickedCard = PickBestPrimarySecondaryOrColorless(booster);
          break;
      }

      return pickedCard;
    }


    private Card BlockPrimaryOrSnatchSecondary(List<Card> booster)
    {
      var outListIfPrimaryIsPicked = CloneScores(Out);
      var outListIfPrimaryIsNotPicked = CloneScores(Out);

      var primaryPick = GetBestCardOfChosenColorsOrColorless(color1: PrimaryColor, cards: booster);

      var secondaryColor = SecondaryColor.HasValue
        ? SecondaryColor.Value
        : In
          .Where(x => x.Color != PrimaryColor)
          .OrderByDescending(x => x.Score)
          .First()
          .Color;

      var secondaryPick = GetBestCardOfChosenColor(secondaryColor, booster);

      var primaryRating = primaryPick != null ? GetRating(primaryPick) : PlayableThreshold;
      var secondaryRating = secondaryPick != null ? GetRating(secondaryPick) : PlayableThreshold;

      if (primaryPick != null && primaryRating > PlayableThreshold)
      {
        if (secondaryPick == null)
        {
          return primaryPick;
        }

        UpdateOut(outListIfPrimaryIsPicked, booster, primaryPick);
        UpdateOut(outListIfPrimaryIsNotPicked, booster, secondaryPick);

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

      return PickBestColorlessOrOffcolor(booster);
    }

    private Card PickBestColorlessOrOffcolor(List<Card> booster)
    {
      return booster
        .OrderBy(x =>
          {
            if (x.IsColorless())
              return 0;

            return 1;
          })
        .ThenByDescending(GetRating)
        .First();
    }

    private Card PickFirstCardOfTheDraft(IEnumerable<Card> booster)
    {
      var card = booster
        .Where(x => !x.IsColorless())
        .OrderByDescending(GetRating)
        .First();

      PrimaryColor = card.Colors[0];
      return card;
    }
  }
}