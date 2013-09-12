namespace Grove.Artifical
{
  using Gameplay;
  using Gameplay.Misc;
  using Gameplay.States;

  public abstract class MachinePlayRule : GameObject
  {    
    public abstract bool Process(int pass, ActivationContext c);
    
    public virtual void Initialize(Game game)
    {
      Game = game;
    }

    protected bool IsEndOfOpponentsTurn(Player controller)
    {
      return Stack.IsEmpty && !controller.IsActive && Turn.Step == Step.EndOfTurn;
    }

    protected bool IsBeforeYouDeclareAttackers(Player controller)
    {
      return Stack.IsEmpty && Turn.Step == Step.BeginningOfCombat && controller.IsActive;
    }

    protected bool IsBeforeYouDeclareBlockers(Player controller)
    {
      return Stack.IsEmpty && Turn.Step == Step.DeclareAttackers && controller.IsActive == false;
    }

    protected bool IsAfterYouDeclareAttackers(Player controller)
    {
      return Stack.IsEmpty && Turn.Step == Step.DeclareAttackers && controller.IsActive;
    }

    protected bool IsAfterYouDeclareBlockers(Player controller)
    {
      return Stack.IsEmpty && Turn.Step == Step.DeclareBlockers && controller.IsActive == false;
    }

    protected bool IsAfterOpponentDeclaresBlockers(Player controller)
    {
      return Stack.IsEmpty && Turn.Step == Step.DeclareBlockers && controller.IsActive;
    }

    protected bool IsAfterOpponentDeclaresAttackers(Player controller)
    {
      return Stack.IsEmpty && Turn.Step == Step.DeclareAttackers && controller.IsActive == false;
    }
  }
}