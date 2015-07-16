namespace Grove
{
  using System.Linq;
  using Events;
  using Infrastructure;

  [Copyable]
  public class TurnEvents : GameObject, IReceive<AttackerJoinedCombatEvent>, IReceive<DamageDealtEvent>,
    IReceive<ZoneChangedEvent>, IReceive<BlockerJoinedCombatEvent>, IReceive<StepStartedEvent>,
    IReceive<EffectPutOnStackEvent>, IReceive<SpellPutOnStackEvent>, IReceive<LifeChangedEvent>, IReceive<AbilityActivatedEvent>
  {
    private readonly TrackableList<Card> _attackers = new TrackableList<Card>();
    private readonly TrackableList<Card> _blockers = new TrackableList<Card>();

    private readonly TrackableList<ZoneChangedEvent> _changedZone = new TrackableList<ZoneChangedEvent>();
    private readonly TrackableList<DamageDealtEvent> _damaged = new TrackableList<DamageDealtEvent>();
    private readonly Trackable<bool> _hasActivePlayerPlayedAnySpell = new Trackable<bool>();
    private readonly Trackable<bool> _hasAnythingBeenPlayedOrActivatedDuringThisStep = new Trackable<bool>();
    private readonly TrackableList<LifeChangedEvent> _lifeChanged = new TrackableList<LifeChangedEvent>();
    private readonly Trackable<bool> _hasActivePlayerAttackedThisTurn = new Trackable<bool>();
    private readonly TrackableList<Card> _planeswalkerActivations = new TrackableList<Card>();

    public bool HasAnythingBeenPlayedOrActivatedDuringThisStep
    {
      get { return _hasAnythingBeenPlayedOrActivatedDuringThisStep.Value; }
    }

    public bool HasActivePlayerPlayedAnySpell
    {
      get { return _hasActivePlayerPlayedAnySpell.Value; }
    }

    public bool HasActivePlayerAttackedThisTurn
    {
      get { return _hasActivePlayerAttackedThisTurn.Value; }
    }

    public void Receive(AttackerJoinedCombatEvent message)
    {
      _attackers.Add(message.Attacker.Card);

      if (message.Attacker.Controller.IsActive)
      {
        _hasActivePlayerAttackedThisTurn.Value = true;
      }
    }

    public void Receive(BlockerJoinedCombatEvent message)
    {
      _blockers.Add(message.Blocker.Card);
    }

    public void Receive(DamageDealtEvent message)
    {
      _damaged.Add(message);
    }

    public void Receive(EffectPutOnStackEvent message)
    {
      _hasAnythingBeenPlayedOrActivatedDuringThisStep.Value = true;
    }

    public void Receive(LifeChangedEvent message)
    {
      _lifeChanged.Add(message);
    }

    public void Receive(SpellPutOnStackEvent message)
    {
      if (message.Controller.IsActive)
      {
        _hasActivePlayerPlayedAnySpell.Value = true;
      }
    }

    public void Receive(StepStartedEvent message)
    {
      _hasAnythingBeenPlayedOrActivatedDuringThisStep.Value = false;
    }

    public void Receive(ZoneChangedEvent message)
    {
      _changedZone.Add(message);
    }

    public void Receive(AbilityActivatedEvent e)
    {
      if (e.Ability.OwningCard.Is().Planeswalker)
      {
        _planeswalkerActivations.Add(e.Ability.OwningCard);
      }
    }

    public void Initialize(Game game)
    {
      Game = game;

      _attackers.Initialize(ChangeTracker);
      _blockers.Initialize(ChangeTracker);

      _changedZone.Initialize(ChangeTracker);
      _damaged.Initialize(ChangeTracker);
      _lifeChanged.Initialize(ChangeTracker);

      _hasAnythingBeenPlayedOrActivatedDuringThisStep.Initialize(ChangeTracker);
      _hasActivePlayerPlayedAnySpell.Initialize(ChangeTracker);
      _hasActivePlayerAttackedThisTurn.Initialize(ChangeTracker);
      _planeswalkerActivations.Initialize(ChangeTracker);
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

    public bool HasLostLife(Player player)
    {
      return _lifeChanged.Any(x => x.Player == player && x.IsLifeLoss);
    }   

    public bool HasAnyLoyalityAbilityBeenActivated(Card card)
    {
      return _planeswalkerActivations.Contains(card);
    }
  }
}