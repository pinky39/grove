namespace Grove.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Costs;
  using Grove.Decisions;
  using Modifiers;

  public class CreaturesOfChosenTypeCostLess : CustomizableEffect
  {
    private readonly IManaAmount _amount;
    private string _mostCommonType;

    private CreaturesOfChosenTypeCostLess() {}

    public CreaturesOfChosenTypeCostLess(IManaAmount amount)
    {
      _amount = amount;
    }

    public override ChosenOptions ChooseResult(List<IEffectChoice> candidates)
    {
      return new ChosenOptions(_mostCommonType);
    }

    public override void ProcessResults(ChosenOptions results)
    {
      var chosenType = (string) results.Options[0];

      var costModifier = new SpellCostModifier(_amount,
        (card, self) => card.Is().Creature && card.Is(chosenType));

      var addCostModifier = new AddCostModifier(costModifier);

      addCostModifier.AddLifetime(new PermanentLeavesBattlefieldLifetime(
        self => self.Modifier.SourceCard
        ));

      var mp = new ModifierParameters
        {
          SourceCard = Source.OwningCard,
          SourceEffect = this
        };

      Game.AddModifier(addCostModifier, mp);
    }

    public override string GetText()
    {
      return "Choose creature type: #0.";
    }

    public override IEnumerable<IEffectChoice> GetChoices()
    {
      var subtypes = new Dictionary<string, int>();

      var cards = Controller.Library.Concat(Controller.Hand).Where(x => x.Is().Creature);

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