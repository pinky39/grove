namespace Grove.Core.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Decisions.Results;
  using Zones;

  public class PlayersSacrificeLands : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly int _count;
    private readonly Func<Effect, Player, bool> _playerFilter;

    private PlayersSacrificeLands() {}

    public PlayersSacrificeLands(int count, Func<Effect, Player, bool> playerFilter = null)
    {
      _count = count;
      _playerFilter = playerFilter ?? delegate { return true; };
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      return candidates
        .OrderBy(x => x.Score)
        .Take(_count)
        .ToList();
    }

    public void ProcessResults(ChosenCards results)
    {
      foreach (var card in results)
      {
        card.Sacrifice();
      }
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
      Enqueue<SelectCards>(
        controller: player,
        init: p =>
          {
            p.MinCount = _count;
            p.MaxCount = _count;
            p.Validator(c => c.Is().Land);
            p.Text = "Select land(s) to sacrifice";
            p.Zone = Zone.Battlefield;
            p.OwningCard = Source.OwningCard;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
          });
    }
  }
}