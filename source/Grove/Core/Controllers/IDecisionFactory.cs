namespace Grove.Core.Controllers
{
  public interface IDecisionFactory
  {
    TDecision CreateHuman<TDecision>();
    TDecision CreateMachine<TDecision>();
  }
}