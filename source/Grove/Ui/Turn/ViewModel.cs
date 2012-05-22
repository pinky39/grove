namespace Grove.Ui.Turn
{
  using System.Collections.Generic;
  using Core;
  using Core.Messages;
  using Infrastructure;

  public class ViewModel : IReceive<TurnStarted>
  {
    private readonly List<Ui.Step.ViewModel> _steps = new List<Ui.Step.ViewModel>();

    public ViewModel(Ui.Step.ViewModel.IFactory stepViewModelFactory)
    {
      _steps.AddRange(CreateStepViewModels(stepViewModelFactory));
      TurnNumber = 1;
    }

    public IEnumerable<Ui.Step.ViewModel> Steps { get { return _steps; } }

    public virtual int TurnNumber { get; protected set; }

    public void Receive(TurnStarted message)
    {
      TurnNumber = (message.TurnCount / 2) + 1;
    }

    private static IEnumerable<Ui.Step.ViewModel> CreateStepViewModels(Ui.Step.ViewModel.IFactory factory)
    {
      yield return factory.Create(Step.Untap, "Untap");
      yield return factory.Create(Step.Upkeep, "Upkeep");
      yield return factory.Create(Step.Draw, "Draw");
      yield return factory.Create(Step.FirstMain, "First main");
      yield return factory.Create(Step.BeginningOfCombat, "Beg. of combat");
      yield return factory.Create(Step.DeclareAttackers, "Dec. attackers");
      yield return factory.Create(Step.DeclareBlockers, "Dec. blockers");
      yield return factory.Create(Step.CombatDamage, "Combat damage");
      yield return factory.Create(Step.EndOfCombat, "End of combat");
      yield return factory.Create(Step.SecondMain, "Second main");
      yield return factory.Create(Step.EndOfTurn, "End of turn");
      yield return factory.Create(Step.CleanUp, "Clean up");
    }
  }
}