namespace Grove.Core.Details.Cards.Effects
{
  using Targeting;

  public class TargetPlayerDiscardsCards : Effect
  {
    public int SelectedCount { get; set; }

    protected override void ResolveEffect()
    {
      Decisions.EnqueueDiscardCards(Target().Player(), SelectedCount);
    }
  }
}