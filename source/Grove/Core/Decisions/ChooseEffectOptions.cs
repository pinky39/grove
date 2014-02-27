namespace Grove.Decisions
{
  using System;
  using System.Collections.Generic;
  using Effects;
  using Events;
  using Grove.UserInterface;

  public class ChooseEffectOptions : Decision
  {
    private readonly Params _p = new Params();

    private ChooseEffectOptions() {}

    public ChooseEffectOptions(Player controller, Action<Params> setParameters) : base(controller,
      () => new UiHandler(), () => new MachineHandler(), () => new MachineHandler(), () => new PlaybackHandler())
    {
      setParameters(_p);
    }

    private abstract class Handler : DecisionHandler<ChooseEffectOptions, ChosenOptions>
    {
      protected override bool ShouldExecuteQuery { get { return true; } }

      public override void ProcessResults()
      {
        D._p.ProcessDecisionResults.ProcessResults(Result);
      }
    }

    private class MachineHandler : Handler
    {
      public MachineHandler()
      {
        Result = new ChosenOptions();
      }

      protected override void ExecuteQuery()
      {
        Result = D._p.ChooseDecisionResults.ChooseResult(D._p.Choices);

        Publish(new EffectOptionsWereChosen
          {
            Text =
              string.Format("{0} chose {1}.", D.Controller, string.Join(", ", Result.Options))
          });
      }
    }

    public class Params
    {
      public List<IEffectChoice> Choices;
      public IChooseDecisionResults<List<IEffectChoice>, ChosenOptions> ChooseDecisionResults;
      public IProcessDecisionResults<ChosenOptions> ProcessDecisionResults;
      public string Text;
    }

    private class PlaybackHandler : Handler
    {
      protected override bool ShouldExecuteQuery { get { return true; } }

      public override void SaveDecisionResults() {}

      protected override void ExecuteQuery()
      {
        Result = (ChosenOptions) Game.Recorder.LoadDecisionResult();
      }
    }

    private class UiHandler : Handler
    {
      protected override void ExecuteQuery()
      {
        var dialog = Ui.Dialogs.EffectChoiceDialog.Create(D._p.Choices, D._p.Text);
        Ui.Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);

        Result = dialog.ChosenOptions;
      }
    }
  }
}