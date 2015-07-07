namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using AI;
  using Decisions;
  using Infrastructure;

  internal class PutSelectedCardIntoPlayIfOpponentGuessedWrong : Effect,
    IProcessDecisionResults<ChosenCards>, IChooseDecisionResults<List<Card>, ChosenCards>,
    IProcessDecisionResults<BooleanResult>, IChooseDecisionResults<BooleanResult>
  {
    private readonly string _question;
    private readonly Func<Card, bool, bool> _isCorrectAnswer;
    private readonly Func<Effect, bool> _chooseAnswer;
    private readonly Trackable<Card> _selected = new Trackable<Card>();
    private readonly Trackable<Card> _selectedTarget = new Trackable<Card>();


    private PutSelectedCardIntoPlayIfOpponentGuessedWrong() {}

    public PutSelectedCardIntoPlayIfOpponentGuessedWrong(
      string question,
      Func<Effect, bool> chooseAnswer,
      Func<Card, bool, bool> isCorrectAnswer)
    {
      _question = question;
      _isCorrectAnswer = isCorrectAnswer;
      _chooseAnswer = chooseAnswer;
    }

    protected override void ResolveEffect()
    {                        
      if (Controller.Hand.Count == 0)
        return;      
      
      Enqueue(new SelectCards(
        Controller,
        p =>
          {
            p.SetValidator(c => true);
            p.Zone = Zone.Hand;
            p.Text = "Select a card.";
            p.OwningCard = Source.OwningCard;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
            p.MinCount = 1;
            p.MaxCount = 1;
            p.AurasNeedTarget = true;
          }));
    }

    protected override void Initialize()
    {
      _selected.Initialize(ChangeTracker);
      _selectedTarget.Initialize(ChangeTracker);
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      return CardPicker
        .ChooseBestCards(
          controller: Controller,
          candidates: candidates,
          count: 1,
          aurasNeedTarget: true);
    }

    public void ProcessResults(ChosenCards results)
    {
      _selected.Value = results[0];
      _selectedTarget.Value = results.Count > 1 ? results[1] : null;

      Enqueue(new ChooseTo(Controller.Opponent, p =>
        {
          p.ProcessDecisionResults = this;
          p.ChooseDecisionResults = this;
          p.Text = _question;
        }));
    }

    public void ProcessResults(BooleanResult results)
    {
      if (_isCorrectAnswer(_selected.Value, results.IsTrue))
        return;

      if (_selectedTarget.Value == null)
      {
        _selected.Value.PutToBattlefield();
        return;
      }

      _selected.Value.EnchantWithoutPayingCost(_selectedTarget.Value);
    }

    public BooleanResult ChooseResult()
    {
      return _chooseAnswer(this);
    }
  }
}