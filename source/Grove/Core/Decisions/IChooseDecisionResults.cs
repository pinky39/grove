namespace Grove.Decisions
{
  using System;

  public class ChooseDecisionResultsHelper<TCandidates, TResult> : IChooseDecisionResults<TCandidates, TResult>
  {
    private readonly Func<TCandidates, TResult> _chooseResult;

    public ChooseDecisionResultsHelper(Func<TCandidates, TResult> chooseResult)
    {
      _chooseResult = chooseResult;
    }

    public TResult ChooseResult(TCandidates candidates)
    {
      return _chooseResult(candidates);
    }   
  }
  
  public interface IChooseDecisionResults<in TCandidates, out TResult>
  {
    TResult ChooseResult(TCandidates candidates);
  }

  public interface IChooseDecisionResults<out TResult>
  {
    TResult ChooseResult();
  }
}