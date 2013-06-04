namespace Grove.UserInterface.Turn
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay.Messages;
  using Gameplay.States;
  using Infrastructure;

  public class ViewModel : ViewModelBase, IReceive<TurnStarted>
  {
    private readonly List<UserInterface.Step.ViewModel> _steps = new List<UserInterface.Step.ViewModel>();

    public IEnumerable<UserInterface.Step.ViewModel> Steps { get { return _steps; } }

    public virtual int TurnNumber { get; protected set; }

    public void Receive(TurnStarted message)
    {
      TurnNumber = message.TurnCount;
    }

    public override void Initialize()
    {
      _steps.AddRange(CreateStepViewModels());
      TurnNumber = CurrentGame.Turn.TurnCount;
      _steps.Single(x => x.IsStep(CurrentGame.Turn.Step)).IsCurent = true;
    }

    private IEnumerable<UserInterface.Step.ViewModel> CreateStepViewModels()
    {
      yield return ViewModels.Step.Create("Untap", Step.Untap);
      yield return ViewModels.Step.Create("Upkeep", Step.Upkeep);
      yield return ViewModels.Step.Create("Draw", Step.Draw);
      yield return ViewModels.Step.Create("First main", Step.FirstMain);
      yield return ViewModels.Step.Create("Beg. of combat", Step.BeginningOfCombat);
      yield return ViewModels.Step.Create("Dec. attackers", Step.DeclareAttackers);
      yield return ViewModels.Step.Create("Dec. blockers", Step.DeclareBlockers);
      yield return ViewModels.Step.Create("Combat damage", Step.CombatDamage, Step.FirstStrikeCombatDamage);
      yield return ViewModels.Step.Create("End of combat", Step.EndOfCombat);
      yield return ViewModels.Step.Create("Second main", Step.SecondMain);
      yield return ViewModels.Step.Create("End of turn", Step.EndOfTurn);
      yield return ViewModels.Step.Create("Clean up", Step.CleanUp);
    }
  }
}