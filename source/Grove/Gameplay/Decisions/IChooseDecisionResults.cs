namespace Grove.Gameplay.Decisions
{
  public interface IChooseDecisionResults<in TCandidates, out TResult>
  {
    TResult ChooseResult(TCandidates candidates);
  }

  public interface IChooseDecisionResults<out TResult>
  {
    TResult ChooseResult();
  }
}