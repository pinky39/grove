namespace Grove.Core.Effects
{
  public class MillOpponent : Effect
  {
    public int Count { get; set; }

    protected override void ResolveEffect()
    {
      var opponent = Players.GetOpponent(Controller);
      opponent.Mill(Count);
    }
  }
}