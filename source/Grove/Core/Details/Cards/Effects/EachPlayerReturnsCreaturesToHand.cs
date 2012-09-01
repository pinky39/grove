namespace Grove.Core.Details.Cards.Effects
{
  using Controllers;

  public class EachPlayerReturnsCreaturesToHand : Effect
  {
    public int Count { get; set; }

    protected override void ResolveEffect()
    {
      ReturnCreatureToHand(Players.Active);
      ReturnCreatureToHand(Players.Passive);
    }

    private void ReturnCreatureToHand(Player player)
    {
      Decisions.Enqueue<ReturnPermanentsToHand>(
        controller: player,
        init: p =>
          {
            p.Count = Count;
            p.Filter = card => card.Is().Creature;
            p.Text = "Select {0} creature(s) to return to hand";
          });
    }
  }
}