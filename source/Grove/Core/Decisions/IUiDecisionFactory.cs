namespace Grove.Core.Controllers
{
  public interface IUiDecisionFactory
  {
    TDecision Create<TDecision>();    
  }
}