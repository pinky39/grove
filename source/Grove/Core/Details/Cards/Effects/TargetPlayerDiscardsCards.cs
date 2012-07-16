namespace Grove.Core.Details.Cards.Effects
{
  using Controllers;
  using Targeting;

  public class TargetPlayerDiscardsCards : Effect
  {
    public int SelectedCount { get; set; }

    protected override void ResolveEffect()
    {
      Decisions.Enqueue<DiscardCards>(
        controller:Target().Player(), 
        init: p => p.Count = SelectedCount);
    }
  }
}