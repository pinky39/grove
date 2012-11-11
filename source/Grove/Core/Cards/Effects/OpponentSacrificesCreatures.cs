namespace Grove.Core.Details.Cards.Effects
{
  using Controllers;

  public class OpponentSacrificesCreatures : Effect
  {
    public int Count { get; set; }

    protected override void ResolveEffect()
    {
      var opponent = Players.GetOpponent(Controller);

      Game.Enqueue<SelectCardsToSacrifice>(
        controller: opponent,
        init: p =>
          {
            p.MinCount = Count;
            p.MaxCount = Count;            
            p.Validator = card => card.Is().Creature;
            p.Text = FormatText("Select creature(s) to sacrifice");            
          });
    }
  }
}