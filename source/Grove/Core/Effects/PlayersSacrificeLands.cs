namespace Grove.Core.Effects
{
  using System;
  using System.Linq;
  using Decisions;
  using Zones;

  public class PlayersSacrificeLands : Effect
  {
    private readonly int _count;
    private readonly Func<Effect, Player, bool> _playerFilter;

    private PlayersSacrificeLands() {}

    public PlayersSacrificeLands(int count, Func<Effect, Player, bool> playerFilter = null)
    {
      _count = count;
      _playerFilter = playerFilter ?? delegate { return true; };
    }

    protected override void ResolveEffect()
    {
      var players = new[] {Players.Active, Players.Passive};

      foreach (var player in players.Where(x => _playerFilter(this, x)))
      {
        SelectCardsToSacrifice(player);
      }
    }

    private void SelectCardsToSacrifice(Player player)
    {
      Enqueue<SelectCardsToSacrifice>(
        controller: player,
        init: p =>
          {
            p.MinCount = _count;
            p.MinCount = _count;
            p.Validator = card => card.Is().Land;
            p.Text = FormatText("Select land(s) to sacrifice");
            p.Zone = Zone.Battlefield;
          });
    }
  }
}