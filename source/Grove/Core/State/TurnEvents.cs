namespace Grove
{
  using System.Linq;
  using Grove.Events;
  using Grove.Infrastructure;

  [Copyable]
  public class TurnEvents : GameObject, IReceive<AttackerJoinedCombat>, IReceive<DamageHasBeenDealt>,
    IReceive<ZoneChanged>, IReceive<BlockerJoinedCombat>, IReceive<StepStarted>,
    IReceive<EffectPushedOnStack>
  {
    private readonly TrackableList<Card> _attackers = new TrackableList<Card>();
    private readonly TrackableList<Card> _blockers = new TrackableList<Card>();
    private readonly TrackableList<ZoneChanged> _changedZone = new TrackableList<ZoneChanged>();
    private readonly TrackableList<DamageHasBeenDealt> _damaged = new TrackableList<DamageHasBeenDealt>();
    private readonly Trackable<bool> _hasSpellsBeenPlayedDuringThisStep = new Trackable<bool>();

    public bool HasSpellsBeenPlayedDuringThisStep
    {
      get { return _hasSpellsBeenPlayedDuringThisStep.Value; }
    }

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

    public void Receive(EffectPushedOnStack message)
    {
      _hasSpellsBeenPlayedDuringThisStep.Value = true;
    }

    public void Receive(StepStarted message)
    {
      _hasSpellsBeenPlayedDuringThisStep.Value = false;
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
      _hasSpellsBeenPlayedDuringThisStep.Initialize(ChangeTracker);
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
      return _changedZone.Any(x => x.Card == card && x.From == from && x.To == to);
    }
  }
}