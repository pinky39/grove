namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using AI;
  using Decisions;
  using Infrastructure;

  internal class CastCardWithoutpayingIfOpponentGuessedWrong : Effect,
    IProcessDecisionResults<ChosenCards>, IChooseDecisionResults<List<Card>, ChosenCards>,
    IProcessDecisionResults<BooleanResult>, IChooseDecisionResults<BooleanResult>
  {
    private readonly string _question;
    private readonly Func<Card, bool, bool> _isCorrectAnswer;
    private readonly Func<Effect, bool> _chooseAnswer;
    private readonly Trackable<Card> _selected = new Trackable<Card>();    

    private CastCardWithoutpayingIfOpponentGuessedWrong() {}

    public CastCardWithoutpayingIfOpponentGuessedWrong(
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
          }));
    }

    protected override void Initialize()
    {
      _selected.Initialize(ChangeTracker);      
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      return CardPicker
        .ChooseBestCards(
          controller: Controller,
          candidates: candidates,
          count: 1,
          aurasNeedTarget: false);
    }

    public void ProcessResults(ChosenCards results)
    {
      _selected.Value = results[0];      

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

      Enqueue(new CastCard(Controller, p =>
        {
          p.Card = _selected.Value;
          p.PayManaCost = false;          
        }));      
    }

    public BooleanResult ChooseResult()
    {
      return _chooseAnswer(this);
    }
  }
}