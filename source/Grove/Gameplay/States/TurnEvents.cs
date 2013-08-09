namespace Grove.Gameplay.States
{
  using System.Linq;
  using Infrastructure;
  using Messages;
  using Misc;
  using Zones;

  [Copyable]
  public class TurnEvents : GameObject, IReceive<AttackerJoinedCombat>, IReceive<DamageHasBeenDealt>,
    IReceive<ZoneChanged>, IReceive<BlockerJoinedCombat>
  {
    private readonly TrackableList<Card> _attackers = new TrackableList<Card>();
    private readonly TrackableList<Card> _blockers = new TrackableList<Card>();
    private readonly TrackableList<ZoneChanged> _changedZone = new TrackableList<ZoneChanged>();
    private readonly TrackableList<DamageHasBeenDealt> _damaged = new TrackableList<DamageHasBeenDealt>();

    public void Receive(AttackerJoinedCombat message)
    {
      _attackers.Add(message.Attacker.Card);
    }

    public void Receive(BlockerJoinedCombat message)
    {
      _blockers.Add(message.Blocker.Card);
    }

    public void Receive(DamageHasBeenDealt message)
    {
      _damaged.Add(message);
    }

    public void Receive(ZoneChanged message)
    {
      _changedZone.Add(message);
    }

    public void Initialize(Game game)
    {
      Game = game;

      _attackers.Initialize(ChangeTracker);
      _blockers.Initialize(ChangeTracker);
      _changedZone.Initialize(ChangeTracker);
      _damaged.Initialize(ChangeTracker);
    }

    public bool HasAttacked(Card card)
    {
      return _attackers.Contains(card);
    }

    public bool HasBlocked(Card card)
    {
      return _blockers.Contains(card);
    }

    public bool HasBeenDamaged(object receiver)
    {
      return _damaged.Any(x => x.Receiver == receiver);
    }

    public bool HasBeenDamagedBy(object receiver, Card damageSource)
    {
      return _damaged.Any(x => x.Receiver == receiver && x.Damage.Source == damageSource);
    }

    public bool HasChangedZone(Card card, Zone from, Zone to)
    {
      return _changedZone.Any(x => x.From == from && x.To == to);
    }
  }
}