namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Events;
  using Infrastructure;
  using Modifiers;

  public class StaticAbility : GameObject, IReceive<ControllerChangedEvent>,
    IReceive<ZoneChangedEvent>, IReceive<PermanentModifiedEvent>, IDisposable,
    ICopyContributor, IHashable
  {
    private readonly bool _enabledInAllZones;
    private readonly Trackable<bool> _isEnabled = new Trackable<bool>();
    private readonly Trackable<bool> _isActivated = new Trackable<bool>();    
    private readonly TrackableList<ManualLifetime> _lifetimes = new TrackableList<ManualLifetime>();
    private readonly Func<StaticAbilityParameters.ConditionParameters, bool> _condition;

    private readonly List<ModifierFactory> _modifierFactories =
      new List<ModifierFactory>();

    private StaticAbility() {}

    public StaticAbility(StaticAbilityParameters p)
    {
      _enabledInAllZones = p.EnabledInAllZones;
      _modifierFactories.AddRange(p.Modifiers);
      _condition = p.Condition;
    }

    public Card OwningCard { get; private set; }

    public Player Controller
    {
      get { return OwningCard.Controller; }
    }

    public void AfterMemberCopy(object original)
    {
      SubscribeToEvents();
    }

    public void Dispose()
    {
      Disable();

      Unsubscribe(this);
      UnsubscribeFromEvents();
    }

    public int CalculateHash(HashCalculator calc)
    {
      return _isEnabled.Value.GetHashCode();
    }

    private bool ShouldBeActivated()
    {
      return _condition == null || _condition(new StaticAbilityParameters.ConditionParameters(OwningCard, Game));
    }

    public void Enable()
    {
      if (!_isActivated && ShouldBeActivated())
      {
        Activate();
      }

      _isEnabled.Value = true;
    }

    private void Activate()
    {
      // should be set at start or there will be an
      // infinite loop
      _isActivated.Value = true;

      foreach (var modifier in _modifierFactories.Select(factory => factory()))
      {
        var p = new ModifierParameters
          {
            IsStatic = true,
            SourceCard = OwningCard
          };

        var lifetime = new ManualLifetime();

        modifier.AddLifetime(lifetime);
        _lifetimes.Add(lifetime);

        if (modifier is IGameModifier)
        {
          Game.AddModifier((IGameModifier) modifier, p);
          continue;
        }

        if (modifier is ICardModifier)
        {
          OwningCard.AddModifier((ICardModifier) modifier, p);
          continue;
        }

        if (modifier is IPlayerModifier)
        {
          OwningCard.Controller.AddModifier((IPlayerModifier) modifier, p);
          continue;
        }
      }
    }

    public void Disable()
    {
      if (_isActivated)
      {
        Deactivate();
      }

      _isEnabled.Value = false;
    }

    private void Deactivate()
    {
      // disable handlers, which will be triggered when
      // ending modifiers lifetimes
      Unsubscribe(this);
      
      // should be set at start or there will be an
      // infinite loop
      _isActivated.Value = false;            

      // create a copy of lifetime to despose
      // since when endlife is called the original
      // can change
      var lifetimes = _lifetimes.ToArray();
      _lifetimes.Clear();

      foreach (var lifetime in lifetimes)
      {
        lifetime.EndLife();
      }

      // enable handlers back
      Subscribe(this);
    }

    private void OnOwningCardJoinedBattlefield()
    {
      if (!_enabledInAllZones)
      {
        Enable();
      }
    }

    private void OnOwningCardLeftBattlefield()
    {
      if (!_enabledInAllZones)
      {
        Disable();
      }
    }

    private void SubscribeToEvents()
    {
      OwningCard.JoinedBattlefield += OnOwningCardJoinedBattlefield;
      OwningCard.LeftBattlefield += OnOwningCardLeftBattlefield;
    }

    private void UnsubscribeFromEvents()
    {
      OwningCard.JoinedBattlefield -= OnOwningCardJoinedBattlefield;
      OwningCard.LeftBattlefield -= OnOwningCardLeftBattlefield;
    }

    public void Initialize(Card owningCard, Game game)
    {
      Game = game;
      OwningCard = owningCard;

      _lifetimes.Initialize(ChangeTracker);
      _isEnabled.Initialize(ChangeTracker);
      _isActivated.Initialize(ChangeTracker);

      if (_enabledInAllZones)
      {
        Enable();
      }

      Subscribe(this);
      SubscribeToEvents();
    }

    public void Receive(ControllerChangedEvent message)
    {
      if (message.Card == OwningCard)
      {
        if (_isActivated)
        {
          // some modifiers needs to be recreated
          // when controler changes
          Disable();
          Enable();
        }
      }
    }

    public void Receive(ZoneChangedEvent e)
    {
      Update();
    }

    private void Update()
    {
      if (_isEnabled && !_isActivated && ShouldBeActivated())
      {
        Activate();
      }
      else if (_isEnabled && _isActivated && !ShouldBeActivated())
      {
        Deactivate();
      }
    }

    public void Receive(PermanentModifiedEvent e)
    {
      Update();
    }
  }
}