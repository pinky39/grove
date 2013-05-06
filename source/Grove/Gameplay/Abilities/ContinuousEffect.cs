namespace Grove.Gameplay.Abilities
{
  using System.Linq;
  using Infrastructure;
  using Messages;
  using Misc;
  using Modifiers;
  using Targeting;
  using Zones;

  public delegate bool ShouldApplyToCard(Card card, ContinuousEffect effect);

  public delegate bool ShouldApplyToPlayer(Player player, ContinuousEffect effect);

  [Copyable]
  public class ContinuousEffect : GameObject, IReceive<ZoneChanged>, IReceive<PermanentWasModified>, ICopyContributor
  {
    private readonly ShouldApplyToCard _cardFilter;
    private readonly Trackable<Modifier> _doNotUpdate = new Trackable<Modifier>();
    private readonly Trackable<bool> _isActive = new Trackable<bool>();
    private readonly ModifierFactory _modifierFactory;
    private readonly TrackableList<Modifier> _modifiers = new TrackableList<Modifier>();
    private readonly ShouldApplyToPlayer _playerFilter;

    private ContinuousEffect() {}

    public ContinuousEffect(ContinuousEffectParameters p)
    {
      _modifierFactory = p.Modifier;
      _cardFilter = p.CardFilter;
      _playerFilter = p.PlayerFilter;
    }

    public Card Source { get; private set; }
    public ITarget Target { get; private set; }

    public void AfterMemberCopy(object original)
    {
      SubscribeToEvents();
    }

    public void Receive(PermanentWasModified message)
    {
      if (ShouldPermanentBeUpdated(message.Card, message.Modifier))
      {
        UpdatePermanent(message.Card);
      }
    }

    public void Receive(ZoneChanged message)
    {
      if (_isActive == false) return;

      if (message.ToBattlefield && _cardFilter(message.Card, this))
      {
        var modifier = FindModifier(message.Card);

        if (modifier == null)
        {
          AddModifier(message.Card);
        }

        return;
      }

      if (message.FromBattlefield)
      {
        var modifier = FindModifier(message.Card);
        if (modifier != null)
        {
          RemoveModifier(modifier);
        }
        return;
      }
    }

    public void Initialize(Card source, Game game, ITarget target = null)
    {
      Game = game;

      _doNotUpdate.Initialize(ChangeTracker);
      _modifiers.Initialize(ChangeTracker);
      _isActive.Initialize(ChangeTracker);

      Source = source;
      Target = target;

      Subscribe(this);

      SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
      Source.JoinedBattlefield += delegate { Activate(); };
      Source.LeftBattlefield += delegate { Deactivate(); };
    }

    private bool ShouldPermanentBeUpdated(Card permanent, IModifier modifier)
    {
      return _isActive && permanent.Zone == Zone.Battlefield && modifier != _doNotUpdate.Value;
    }

    private void UpdatePermanent(Card permanent)
    {
      var modifier = FindModifier(permanent);
      var shouldEffectBeApliedToPermanent = _cardFilter(permanent, this);

      if (modifier == null && shouldEffectBeApliedToPermanent)
      {
        AddModifier(permanent);
        return;
      }

      if (modifier != null && !shouldEffectBeApliedToPermanent)
      {
        RemoveModifier(modifier);
      }
    }

    private void RemoveModifier(Modifier modifier)
    {
      _doNotUpdate.Value = modifier;

      _modifiers.Remove(modifier);
      modifier.Remove();
    }

    private Modifier FindModifier(Card permanent)
    {
      return _modifiers
        .FirstOrDefault(x => x.Target == permanent);
    }

    public void Activate()
    {
      AddModifierToPermanents();
      AddModifierToPlayers();
      _isActive.Value = true;
    }

    private void AddModifierToPlayers()
    {
      foreach (var player in Players)
      {
        if (_playerFilter(player, this))
        {
          AddModifier(player);
        }
      }
    }

    private void AddModifier(ITarget target)
    {
      var p = new ModifierParameters
        {
          SourceCard = Source,
          Target = target
        };

      var modifier = _modifierFactory().Initialize(p, Game);
      _modifiers.Add(modifier);

      _doNotUpdate.Value = modifier;
      target.AddModifier(modifier);
    }

    private void AddModifierToPermanents()
    {
      var permanents = Players.Permanents()
        .Where(permanent => _cardFilter(permanent, this)).ToList();

      foreach (var permanent in permanents)
      {
        AddModifier(permanent);
      }
    }

    public void Deactivate()
    {
      _isActive.Value = false;

      foreach (var modifier in _modifiers.ToList())
      {
        RemoveModifier(modifier);
      }
    }
  }
}