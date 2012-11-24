namespace Grove.Core.Cards.Effects
{
  using Decisions;
  using Zones;

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
      Game.Enqueue<SelectCardsPutToHand>(
        controller: player,
        init: p =>
          {
            p.MinCount = Count;
            p.MinCount = Count;
            p.Validator = card => card.Is().Creature;
            p.Zone = Zone.Battlefield;
            p.Text = "Select creature(s) to return to hand";
          });
    }
  }
}