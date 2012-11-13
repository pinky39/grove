namespace Grove.Core.Decisions
{
  public interface IUiDecisionFactory
  {
    TDecision Create<TDecision>();    
  }
}