namespace Grove.Core.Effects
{
  public class MillOpponent : Effect
  {
    public int Count { get; set; }
        
    public override void Resolve()
    {
      var opponent = Players.GetOpponent(Controller);
      opponent.Mill(Count);
    }
  }
}