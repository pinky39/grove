namespace Grove.Core.Details.Cards.Effects
{
  using Targeting;

  public class DiscardCards : Effect
  {
    public int Count { get; set; }
    public Player ChosenPlayer { get; set; }

    public override bool NeedsTargets { get { return ChosenPlayer == null; } }

    protected override void ResolveEffect()
    {
      var player = ChosenPlayer ?? Target().Player();

      Decisions.Enqueue<Controllers.DiscardCards>(
        controller: player,
        init: p => p.Count = Count);
    }
  }
}