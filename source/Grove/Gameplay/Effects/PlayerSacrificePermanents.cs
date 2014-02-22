namespace Grove.Gameplay.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;

  public class PlayerSacrificePermanents : Effect,
    IProcessDecisionResults<ChosenCards>, IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly int _count;
    private readonly DynParam<Player> _player;
    private readonly Func<Card, bool> _filter;
    private readonly string _text;

    private PlayerSacrificePermanents() {}

    public PlayerSacrificePermanents(int count, DynParam<Player> player, Func<Card, bool> filter, string text)
    {
      _count = count;
      _player = player;
      _filter = filter;
      _text = text;

      RegisterDynamicParameters(_player);
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
      Enqueue(new SelectCards(
        _player.Value,
        p =>
          {
            p.MinCount = _count;
            p.MaxCount = _count;
            p.SetValidator(_filter);
            p.Zone = Zone.Battlefield;
            p.Text = _text;
            p.OwningCard = Source.OwningCard;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
          }));
    }
  }
}