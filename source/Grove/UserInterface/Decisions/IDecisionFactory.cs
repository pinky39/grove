namespace Grove.UserInterface.Decisions
{
  public interface IDecisionFactory
  {
    TDecision CreateUi<TDecision>();    
  }
}