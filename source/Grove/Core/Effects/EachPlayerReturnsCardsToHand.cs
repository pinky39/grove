namespace Grove.Core.Effects
{
  using System;
  using Grove.Core.Decisions;
  using Grove.Core.Zones;

  public class EachPlayerReturnsCardsToHand : Effect
  {
    public int MinCount { get; set; }
    public int MaxCount { get; set; }
    public Func<Card, bool> Filter = delegate { return true; };
    public Zone Zone;
    public bool AiOrdersByDescendingScore;
    public string Text;

    protected override void ResolveEffect()
    {
      ReturnCreatureToHand(Core.Players.Active);
      ReturnCreatureToHand(Core.Players.Passive);
    }

    private void ReturnCreatureToHand(Player player)
    {
      Game.Enqueue<SelectCardsPutToHand>(
        controller: player,
        init: p =>
          {
            p.MinCount = MinCount;
            p.MaxCount = MaxCount;
            p.Validator = Filter;
            p.Zone = Zone;
            p.AiOrdersByDescendingScore = AiOrdersByDescendingScore;
            p.Text = Text;
          });
    }
  }
}