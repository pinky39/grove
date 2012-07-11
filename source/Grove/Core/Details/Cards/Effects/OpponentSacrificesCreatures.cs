namespace Grove.Core.Details.Cards.Effects
{
  public class OpponentSacrificesCreatures : Effect
  {
    public int Count { get; set; }

    protected override void ResolveEffect()
    {
      var opponent = Players.GetOpponent(Controller);
      Decisions.EnqueueSacrificeCreatures(opponent, Count);
    }
  }
}