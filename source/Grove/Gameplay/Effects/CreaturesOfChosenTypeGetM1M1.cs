namespace Grove.Gameplay.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Abilities;
  using Decisions.Results;
  using Modifiers;

  public class CreaturesOfChosenTypeGetM1M1 : CustomizableEffect
  {
    private string _mostCommonType;

    public CreaturesOfChosenTypeGetM1M1()
    {
      ToughnessReduction = 1;
    }

    public override ChosenOptions ChooseResult(List<IEffectChoice> candidates)
    {
      return new ChosenOptions(_mostCommonType);
    }

    public override void ProcessResults(ChosenOptions results)
    {
      var chosenType = (string)results.Options[0];

      var cp = new ContinuousEffectParameters
        {
          Modifier = () => new AddPowerAndToughness(-1, -1),
          CardFilter = (card, effect) => card.Is().Creature && card.Is(chosenType)
        };

      var modifier = new AddContiniousEffect(
        new ContinuousEffect(cp));


      var mp = new ModifierParameters
        {
          SourceCard = Source.OwningCard,
          SourceEffect = this
        };

      Source.OwningCard.AddModifier(modifier, mp);
    }

    public override string GetText()
    {
      return "Choose creature type: #0.";
    }

    public override IEnumerable<IEffectChoice> GetChoices()
    {
      var subtypes = new Dictionary<string, int>();

      var cards = Controller.Opponent.Library.Concat(Controller.Opponent.Battlefield).Where(x => x.Is().Creature);

      foreach (var card in cards)
      {
        foreach (var subType in card.Subtypes)
        {
          if (!subtypes.ContainsKey(subType))
            subtypes.Add(subType, 1);
          else
          {
            subtypes[subType]++;
          }
        }
      }

      if (subtypes.Count == 0)
      {
        subtypes["elf"] = 1;
      }

      _mostCommonType = subtypes.OrderByDescending(x => x.Value).First().Key;
      yield return new DiscreteEffectChoice(subtypes.Keys.ToArray());
    }
  }
}