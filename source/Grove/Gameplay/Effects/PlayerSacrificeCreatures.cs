namespace Grove.Gameplay.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Decisions.Results;
  using Zones;

  public class PlayerSacrificeCreatures : Effect,
    IProcessDecisionResults<ChosenCards>, IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly int _count;
    private readonly DynParam<Player> _player;

    private PlayerSacrificeCreatures() {}

    public PlayerSacrificeCreatures(int count, DynParam<Player> player)
    {
      _count = count;
      _player = player;

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
      Enqueue<SelectCards>(
        controller: _player.Value,
        init: p =>
          {
            p.MinCount = _count;
            p.MaxCount = _count;
            p.Validator(card => card.Is().Creature);
            p.Zone = Zone.Battlefield;
            p.Text = "Select creature(s) to sacrifice.";
            p.OwningCard = Source.OwningCard;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
          });
    }
  }
}