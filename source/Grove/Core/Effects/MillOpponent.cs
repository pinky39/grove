namespace Grove.Core.Effects
{
  public class MillOpponent : Effect
  {
    public int Count { get; set; }

    protected override void ResolveEffect()
    {
      var opponent = Core.Players.GetOpponent(Controller);
      opponent.Mill(Count);
    }
  }
}