namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Decisions;

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
      var count = _count.Value;

      return candidates
        .OrderBy(x => x.Score)
        .Take(count)
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
      Enqueue(new SelectCards(
        player,
        p =>
          {
            p.MinCount = _count;
            p.MaxCount = _count;
            p.SetValidator(_validator);
            p.Text = _text;
            p.Zone = Zone.Battlefield;
            p.OwningCard = Source.OwningCard;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
          }));
    }
  }
}