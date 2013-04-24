namespace Grove.Core.Decisions
{
  public interface IChooseDecisionResults<in TCandidates, out TResult>
  {
    TResult ChooseResult(TCandidates candidates);
  }
}