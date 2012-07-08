namespace Grove.Core.Effects
{
  public class TargetPlayerDiscardsCards : Effect
  {
    public int SelectedCount { get; set; }
    
    protected override void ResolveEffect()
    {
      Decisions.EnqueueDiscardCards(Target().Player(), SelectedCount);
    }
  }
}