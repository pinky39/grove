namespace Grove.Decisions
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using Infrastructure;
  using UserInterface;

  public class CastCard : Decision
  {
    private readonly Params _p = new Params();
    
    private CastCard() { }

    public CastCard(Player controller, Action<Params> setParameters) : base(controller, 
      () => new UiHandler(), () => new MachineHandler(), () => new ScenarioHandler(),
      () => new PlaybackHandler())
    {
      setParameters(_p);
    }

    private abstract class Handler : DecisionHandler<CastCard, ChosenPlayable>
    {
      protected Handler()
      {
        Result = ChosenPlayable.Pass;
      }
      
      public override void ProcessResults()
      {                  
        Result.Playable.Play();
      }
    }

    private class MachineHandler : Handler, ISearchNode, IMachineExecutionPlan
    {
      private readonly MachinePlanExecutor _executor;
      private List<IPlayable> _playables;

      public MachineHandler()
      {        
        _executor = new MachinePlanExecutor(this);
      }

      public override bool HasCompleted { get { return _executor.HasCompleted; } }     

      void IMachineExecutionPlan.ExecuteQuery()
      {
        ExecuteQuery();
      }

      Game ISearchNode.Game { get { return Game; } }
      public Player Controller { get { return D.Controller; } }

      public int ResultCount { get { return _playables.Count; } }

      public void GenerateChoices()
      {
        _playables = GeneratePlayables();        
      }

      public void SetResult(int index)
      {
        Result = new ChosenPlayable { Playable = _playables[index] };
        LogFile.Debug("Move is {0}", _playables[index]);
      }

      public override void Execute()
      {
        _executor.Execute();
      }

      public override string ToString()
      {
        return string.Format("{0}, {1} casts {2}", 
          Game.Turn.Step, Controller, D._p.Card);
      }

      protected override void Initialize()
      {
        _executor.Initialize(ChangeTracker);
      }

      protected override void ExecuteQuery()
      {
        Ai.SetBestResult(this);
      }

      private List<IPlayable> GeneratePlayables()
      {       
        return new PlayableGenerator(Controller, Game)
          .GetPlayablesForSpell(D._p.Card, 
            respectTimingRules: false, 
            payManaCost: D._p.PayManaCost);
      }
    }

    private class PlaybackHandler : Handler
    {      
      public override void SaveDecisionResults() { }

      protected override void ExecuteQuery()
      {
        Result = (ChosenPlayable)Game.Recorder.LoadDecisionResult();
      }
    }

    private class ScenarioHandler : Handler
    {
      protected override void ExecuteQuery()
      {
        ExecuteAssertions();
        Result = GetNextScenarioResult() ?? ChosenPlayable.Pass;
      }
    }

    private class UiHandler : Handler
    {
      protected override void ExecuteQuery()
      {
        var activationParameters = new ActivationParameters
          {
            PayManaCost = D._p.PayManaCost
          };

        var spellPrerequisites = SelectSpell();
        if (spellPrerequisites == null)
          return;

        UiHelpers.SelectX(spellPrerequisites, activationParameters, canCancel: false);
        UiHelpers.SelectTargets(spellPrerequisites, activationParameters, canCancel: false);

        var playable = new PlayableSpell
          {
            Card = D._p.Card,
            ActivationParameters = activationParameters,
            Index = spellPrerequisites.Index
          };

        Result = new ChosenPlayable {Playable = playable};        
      }

      private ActivationPrerequisites SelectSpell()
      {
        var prerequisites = D._p.Card
          .GetCastPrerequisites(D._p.PayManaCost)
          .Where(x => x.CanBePlayedRegardlessofTime)
          .ToList();        

        if (prerequisites.Count == 0)
          return null;

        if (prerequisites.Count == 1)
          return prerequisites[0];

        var dialog = Ui.Dialogs.SelectAbility.Create(prerequisites.Select(x => x.Description), canCancel: false);
        Ui.Shell.ShowModalDialog(dialog, DialogType.Large, InteractionState.Disabled);

        return prerequisites[dialog.SelectedIndex];
      }
    }

    [Copyable]
    public class Params
    {
      public Card Card;
      public bool PayManaCost = true;
    }

  }
}