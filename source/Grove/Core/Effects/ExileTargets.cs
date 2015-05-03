namespace Grove.Effects
{
  using AI;

  public class ExileTargets : Effect
  {
    public ExileTargets()
    {
      SetTags(EffectTag.Exile);
    }

    protected override void ResolveEffect()
    {
      foreach (var target in ValidEffectTargets)
      {
        target.Card().Exile();
      }
    }
  }
}