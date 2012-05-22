namespace Grove.Core.Effects
{
  public class OpponentSacrificesCreatures : Effect
  {
    public int Count { get; set; }

    public override void Resolve()
    {
      var opponent = Players.GetOpponent(Controller);
      Decisions.EnqueueSacrificeCreatures(opponent, Count);
    }
  }
}