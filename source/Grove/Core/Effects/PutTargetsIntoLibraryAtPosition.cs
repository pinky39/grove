namespace Grove.Effects
{
  public class PutTargetsIntoLibraryAtPosition : Effect
  {
    private readonly int _positionFromTheTop;

    private PutTargetsIntoLibraryAtPosition() {}

    public PutTargetsIntoLibraryAtPosition(int positionFromTheTop)
    {
      _positionFromTheTop = positionFromTheTop;
    }

    protected override void ResolveEffect()
    {
      foreach (var target in ValidEffectTargets)
      {
        target.Controller().PutCardIntoLibraryAtPosition(
          _positionFromTheTop, target.Card());
      }
    }

  }
}

