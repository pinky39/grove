namespace Grove.Core.Controllers.Human
{
  using System.Linq;
  using Infrastructure;
  using Results;
  using Ui;
  using Ui.SelectTarget;
  using Ui.Shell;

  public class DeclareBlockers : Controllers.DeclareBlockers
  {
    public ViewModel.IFactory DialogFactory { get; set; }
    public IShell Shell { get; set; }
    public Publisher Publisher { get { return Game.Publisher; } }

    protected override void ExecuteQuery()
    {
      var result = new ChosenBlockers();

      while (true)
      {
        var selectBlocker = DialogFactory.Create(
          new UiTargetSelector(
            minTargetCount: 0,
            maxTargetCount: 1,
            text: "Select a blocker.",
            isValid: target => target.CanBlock() &&
              target.Controller == Controller
            ), canCancel: false, instructions: "(Press spacebar to finish.)"
          );

        Shell.ShowModalDialog(selectBlocker, DialogType.Small, SelectionMode.SelectTarget);

        if (selectBlocker.Selection.Count() == 0)
          break;

        var blocker = selectBlocker.Selection[0].Card();

        if (result.ContainsBlocker(blocker))
        {
          result.Remove(blocker);

          Publisher.Publish(new BlockerUnselected
            {
              Blocker = blocker
            });

          continue;
        }

        var selectAttacker = DialogFactory.Create(
          new UiTargetSelector(
            minTargetCount: 1,
            maxTargetCount: 1,
            text: "Select an attacker to block.",
            isValid: target => Game.Combat.IsAttacker(target) && target.CanBeBlockedBy(blocker)
            ),
          canCancel: true,
          instructions: "(Press Esc to cancel.)"
          );

        Shell.ShowModalDialog(selectAttacker, DialogType.Small, SelectionMode.SelectTarget);

        if (selectAttacker.WasCanceled)
          continue;

        var attacker = selectAttacker.Selection[0].Card();

        Publisher.Publish(new BlockerSelected
          {
            Blocker = blocker,
            Attacker = attacker
          });

        result.Add(blocker, attacker);
      }

      Result = result;
    }
  }
}