namespace Grove.Gameplay.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Decisions.Results;
  using Zones;

  [Serializable]
  public class PlayersSacrificePermanents : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly DynParam<int> _count;
    private readonly Func<Effect, Player, bool> _playerFilter;
    private readonly string _text;
    private readonly Func<Card, bool> _validator;

    private PlayersSacrificePermanents() {}

    public PlayersSacrificePermanents(DynParam<int> count, string text, Func<Card, bool> validator = null,
      Func<Effect, Player, bool> playerFilter = null)
    {
      _count = count;
      _validator = validator ?? delegate { return true; };
      _text = text;
      _playerFilter = playerFilter ?? delegate { return true; };

      RegisterDynamicParameters(count);
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
            p.Validator(_validator);
            p.Text = _text;
            p.Zone = Zone.Battlefield;
            p.OwningCard = Source.OwningCard;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
          });
    }
  }
}