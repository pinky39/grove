namespace Grove.Core
{
  using System.Linq;
  using Infrastructure;

  public static class Permanents
  {
    public static void MoveDeadCreaturesToGraveyard(this Players players)
    {
      foreach (var player in players)
      {
        player.MoveCreaturesWithLeathalDamageOrZeroTougnessToGraveyard();
      }

      players.RespectLegendaryRule();
    }

    public static void RemoveDamageFromPermanents(this Players players)
    {
      foreach (var player in players)
      {
        player.RemoveDamageFromPermanents();
      }
    }

    public static void RemoveRegenerationFromPermanents(this Players players)
    {
      foreach (var player in players)
      {
        player.RemoveRegenerationFromPermanents();
      }
    }

    private static void RespectLegendaryRule(this Players players)
    {
      var duplicateLegends = players
          .SelectMany(x => x.Battlefield.Legends)
          .GetDuplicates(card => card.Name).ToArray();

      foreach (var legend in duplicateLegends)
      {
        var controller = legend.Controller;
        controller.SacrificeCard(legend);
      }
    }
  }
}