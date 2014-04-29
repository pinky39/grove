namespace Grove.UserInterface.Steps
{
  using System.Collections.Generic;
  using System.Linq;
  using Events;
  using Grove.Infrastructure;
  using Step = Grove.Step;

  public class ViewModel : ViewModelBase, IReceive<TurnStartedEvent>
  {
    private readonly List<UserInterface.Step.ViewModel> _steps = new List<UserInterface.Step.ViewModel>();

    public IEnumerable<UserInterface.Step.ViewModel> Steps { get { return _steps; } }

    public virtual int TurnNumber { get; protected set; }

    public void Receive(TurnStartedEvent message)
    {
      TurnNumber = message.TurnCount;
    }

    public override void Initialize()
    {
      _steps.AddRange(CreateStepViewModels());
      TurnNumber = Game.Turn.TurnCount;

      SetCurrentStep();
    }

    private void SetCurrentStep() {
      var currentStep = _steps.SingleOrDefault(x => x.IsStep(Game.Turn.Step));
      
      if (currentStep != null)  // pre untap steps are not shown (e.g Mulligan step)
        currentStep.IsCurent = true;
    }

    private IEnumerable<UserInterface.Step.ViewModel> CreateStepViewModels()
    {
      yield return ViewModels.Step.Create("Untap", Step.Untap);
      yield return ViewModels.Step.Create("Upkeep", Step.Upkeep);
      yield return ViewModels.Step.Create("Draw", Step.Draw);
      yield return ViewModels.Step.Create("1st main", Step.FirstMain);
      yield return ViewModels.Step.Create("Beg. of combat", Step.BeginningOfCombat);
      yield return ViewModels.Step.Create("Dec. attackers", Step.DeclareAttackers);
      yield return ViewModels.Step.Create("Dec. blockers", Step.DeclareBlockers);
      yield return ViewModels.Step.Create("Combat damage", Step.CombatDamage, Step.FirstStrikeCombatDamage);
      yield return ViewModels.Step.Create("End of combat", Step.EndOfCombat);
      yield return ViewModels.Step.Create("2nd main", Step.SecondMain);
      yield return ViewModels.Step.Create("End of turn", Step.EndOfTurn);
      yield return ViewModels.Step.Create("Clean up", Step.CleanUp);
    }
  }
}