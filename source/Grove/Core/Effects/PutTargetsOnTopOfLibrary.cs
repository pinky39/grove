namespace Grove.Effects
{
  public class PutTargetsOnTopOfLibrary : Effect
  {
    private readonly int _skipCount;

    private PutTargetsOnTopOfLibrary() {}

    public PutTargetsOnTopOfLibrary(int skipCount = 0)
    {
      _skipCount = skipCount;
    }

    protected override void ResolveEffect()
    {
      foreach (var target in ValidEffectTargets)
      {
        if (_skipCount == 0)
        {
          target.Card().PutOnTopOfLibrary();
        }
        else
        {
          target.Controller().PutCardOnTopOfLibrary(target.Card(), _skipCount);
        }
      }            
    }
  }
}