namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Decisions;
  using Modifiers;

  public class CreaturesOfChosenTypeGainPT : CustomizableEffect
  {
    private string _mostCommonType;
    private readonly int _power;
    private readonly int _toughness;
    private readonly ControlledBy _controlledBy;


    private CreaturesOfChosenTypeGainPT() {}

    public CreaturesOfChosenTypeGainPT(int power, int toughness, ControlledBy controlledBy = ControlledBy.Any)
    {
      _power = power;
      _toughness = toughness;
      _controlledBy = controlledBy;

      if (_toughness < 0)
      {
        ToughnessReduction = Math.Abs(_toughness);
      }
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
          Modifier = () => new AddPowerAndToughness(_power, _toughness),
          CardFilter = (card, effect) => card.Is().Creature && card.Is(chosenType) && IsValidController(card)
        };

      var modifier = new AddContiniousEffect(
        new ContinuousEffect(cp));


      var mp = new ModifierParameters
        {
          SourceCard = Source.OwningCard,
          SourceEffect = this
        };

      Source.OwningCard.Controller.AddModifier(modifier, mp);
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

    private bool IsValidController(Card card)
    {
      if (_controlledBy == ControlledBy.Any)
        return true;

      if (_controlledBy == ControlledBy.SpellOwner)
        return card.Controller == Controller;

      return card.Controller == Controller.Opponent;
    }
  }
}