namespace Grove.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using Decisions;

  public class SacrificeCreatureOrPayLifeOrOpponentDrawsCard : CustomizableEffect,
    IProcessDecisionResults<ChosenCards>, IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly int _lifeAmount;

    private SacrificeCreatureOrPayLifeOrOpponentDrawsCard() {}

    public SacrificeCreatureOrPayLifeOrOpponentDrawsCard(int lifeAmount)
    {
      _lifeAmount = lifeAmount;
    }

    protected override Player SelectChoosingPlayer()
    {
      return Controller.Opponent;
    }

    private class ScoreAndOption
    {
      public int Score;
      public EffectOption Option;
    }

    public override ChosenOptions ChooseResult(List<IEffectChoice> candidates)
    {
      var opponent = Controller.Opponent;
      var options = new List<ScoreAndOption>
        {
          new ScoreAndOption
            {
              Score = ScoreCalculator.CalculateLifelossScore(opponent.Life, _lifeAmount),
              Option = EffectOption.PayLife
            },
          new ScoreAndOption
            {
              Score = ScoreCalculator.HiddenCardInHandScore,
              Option = EffectOption.OpponentDrawsACard
            }          
        };

      if (candidates[0].Options.Contains(EffectOption.SacrificeACreature))
      {
        options.Add(new ScoreAndOption
          {
            Score = SelectACreature().Score,
            Option = EffectOption.SacrificeACreature
          });
      }

      return new ChosenOptions(options.OrderBy(x => x.Score).First().Option);
    }

    private Card SelectACreature()
    {
      return Controller.Opponent.Battlefield.Creatures
        .OrderBy(x => x.Score)
        .First();
    }

    public override void ProcessResults(ChosenOptions results)
    {
      var option = (EffectOption) results.Options[0];

      if (option == EffectOption.SacrificeACreature)
      {
        Enqueue(new SelectCards(
          Controller.Opponent,
          p =>
            {
              p.SetValidator(card => card.Is().Creature);
              p.Zone = Zone.Battlefield;
              p.Text = "Select a creature to sacrifice.";
              p.OwningCard = Source.OwningCard;
              p.ProcessDecisionResults = this;
              p.ChooseDecisionResults = this;
              p.MinCount = 1;
              p.MaxCount = 1;
            }));
        return;
      }

      if (option == EffectOption.OpponentDrawsACard)
      {
        Controller.DrawCard();
        return;
      }

      Controller.Opponent.Life -= _lifeAmount;
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      return new ChosenCards(SelectACreature());
    }

    public void ProcessResults(ChosenCards results)
    {
      results[0].Sacrifice();
    }

    public override string GetText()
    {
      return "Select an effect: #0.";
    }

    public override IEnumerable<IEffectChoice> GetChoices()
    {
      if (Controller.Opponent.Battlefield.Creatures.Any())
      {
        yield return new DiscreteEffectChoice(
          EffectOption.PayLife,
          EffectOption.SacrificeACreature,
          EffectOption.OpponentDrawsACard);
      }
      else
      {
        yield return new DiscreteEffectChoice(
          EffectOption.PayLife,
          EffectOption.OpponentDrawsACard);
      }
    }
  }
}