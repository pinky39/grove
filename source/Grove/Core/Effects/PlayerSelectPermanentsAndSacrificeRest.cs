namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;

  public class PlayerSelectPermanentsAndSacrificeRest : Effect,
    IProcessDecisionResults<ChosenCards>, IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly DynParam<int> _toUpCount;
    private readonly DynParam<Player> _player;
    private readonly Func<Card, bool> _filter;
    private readonly string _text;

    private PlayerSelectPermanentsAndSacrificeRest() {}

    public PlayerSelectPermanentsAndSacrificeRest(DynParam<int> toUpCount, DynParam<Player> player, 
      Func<Card, bool> filter, string text)
    {
      _toUpCount = toUpCount;
      _player = player;
      _filter = filter;
      _text = text;

      RegisterDynamicParameters(_player, _toUpCount);
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      return candidates
        .OrderBy(x => -x.Score)
        .Take(_toUpCount)
        .ToList();
    }

    public void ProcessResults(ChosenCards results)
    {
      var permanents = _player.Value.Battlefield
        .Where(_filter)
        .Where(x => !results.Contains(x))
        .ToList();

      foreach (var card in permanents)
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
            p.MinCount = 0;
            p.MaxCount = _toUpCount;
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
