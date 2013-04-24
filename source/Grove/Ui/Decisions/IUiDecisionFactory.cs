namespace Grove.Ui.Decisions
{
  public interface IUiDecisionFactory
  {
    TDecision Create<TDecision>();    
  }
}