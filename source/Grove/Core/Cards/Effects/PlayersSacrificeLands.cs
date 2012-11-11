namespace Grove.Core.Details.Cards.Effects
{
  using Controllers;

  public class PlayersSacrificeLands : Effect
  {
    public int Count;
    public Player OnlyPlayer;

    protected override void ResolveEffect()
    {
      if (OnlyPlayer != null)
      {
        SelectCardsToSacrifice(OnlyPlayer);
        return;
      }

      var players = new[] {Players.Active, Players.Passive};

      foreach (var player in players)
      {
        SelectCardsToSacrifice(player);
      }
    }

    private void SelectCardsToSacrifice(Player player)
    {
      Game.Enqueue<SelectCardsToSacrifice>(
        controller: player,
        init: p =>
          {
            p.MinCount = Count;
            p.MinCount = Count;
            p.Validator = card => card.Is().Land;
            p.Text = "Select land(s) to sacrifice";
          });
    }
  }
}