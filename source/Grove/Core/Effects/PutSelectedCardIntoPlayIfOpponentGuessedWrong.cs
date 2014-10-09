namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Infrastructure;

  class PutSelectedCardIntoPlayIfOpponentGuessedWrong : CustomizableEffect,
        IProcessDecisionResults<ChosenCards>, IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly Func<Card, bool> _ifGuessed;
    private Card _selectedCard;

    private PutSelectedCardIntoPlayIfOpponentGuessedWrong() {}

    public PutSelectedCardIntoPlayIfOpponentGuessedWrong(Func<Card, bool> ifGuessed = null)
    {
      _ifGuessed = ifGuessed ?? delegate { return true; };
    }

    protected override void ResolveEffect()
    {
      Enqueue(new SelectCards(
                  Controller,
                  p =>
                  {
                    p.SetValidator(card => card.ConvertedCost > 0);
                    p.Zone = Zone.Hand;
                    p.Text = "Select a card. Opponent will guesses its converted cost.";
                    p.OwningCard = Source.OwningCard;
                    p.ProcessDecisionResults = this;
                    p.ChooseDecisionResults = this;
                    p.MinCount = 1;
                    p.MaxCount = 1;
                  }));
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      return candidates
        .OrderBy(c => c.Is().Creature ? 1 : 2)
        .ThenBy(c => c.Score)
        .Take(1)
        .ToList();      
    }

    public void ProcessResults(ChosenCards results)
    {
      _selectedCard = results.FirstOrDefault();

      if (results.None())
      {
        return;
      }

      Enqueue(new ChooseEffectOptions(Controller.Opponent, p =>
      {
        p.ProcessDecisionResults = this;
        p.ChooseDecisionResults = this;
        p.Text = GetText();
        p.Choices = GetChoices().ToList();
      }));
    }

    public override ChosenOptions ChooseResult(List<IEffectChoice> candidates)
    {
      var answer = Game.Turn.TurnCount > 10 ? "True" : "False";

      return new ChosenOptions(answer);
    }

    public override void ProcessResults(ChosenOptions results)
    {
      var answerTrue = ((string)results.Options[0]).Equals("true", StringComparison.OrdinalIgnoreCase);

      if (answerTrue && _ifGuessed(_selectedCard) ||
          !answerTrue && !_ifGuessed(_selectedCard))
      {
        if (_selectedCard.Is().Creature)
        {
          _selectedCard.PutToBattlefield();
        }
        else
        {
          throw new NotSupportedException("Cannot play card type " + _selectedCard.Type);
        }
//        var ap = _selectedCard.CanCast().FirstOrDefault();
//
//        if (ap != null)
//        {
//          if (ap.Selector.RequiresTargets)
//          {
//            var targets = ap.Selector.GenerateCandidates();
//
//            if ((ap.Selector.RequiresEffectTargets && !targets.HasEffect) ||
//                (ap.Selector.RequiresCostTargets && !targets.HasCost))
//            {
//              return;
//            }
//          }
//
//          // TODO: Add targets
//          _selectedCard.Cast(0, new ActivationParameters()
//          {
//            PayCost = false,
//          });
//        }         
      }
    }

    public override string GetText()
    {
      return "Is converted cost of selected card greater than 4? Answer: #0.";
    }

    public override IEnumerable<IEffectChoice> GetChoices()
    {
      yield return new BooleanEffectChoice();
    }
  }
}
