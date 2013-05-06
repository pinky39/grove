namespace Grove.UserInterface.Decisions
{
  public interface IUiDecisionFactory
  {
    TDecision Create<TDecision>();
  }
}