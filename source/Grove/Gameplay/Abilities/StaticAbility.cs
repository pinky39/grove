namespace Grove.Gameplay.Abilities
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Messages;
  using Misc;
  using Modifiers;

  public class StaticAbility : GameObject, IReceive<ControllerChanged>, 
    IDisposable, ICopyContributor, IHashable
  {
    private readonly bool _enabledInAllZones;
    private readonly Trackable<bool> _isEnabled = new Trackable<bool>();
    private readonly TrackableList<ManualLifetime> _lifetimes = new TrackableList<ManualLifetime>();

    private readonly List<ModifierFactory> _modifierFactories =
      new List<ModifierFactory>();

    private StaticAbility() {}

    public StaticAbility(StaticAbilityParamaters p)
    {
      _enabledInAllZones = p.EnabledInAllZones;
      _modifierFactories.AddRange(p.Modifiers);
    }

    public Card OwningCard { get; private set; }
    public Player Controller { get { return OwningCard.Controller; } }

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

    public void Enable()
    {
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

      _isEnabled.Value = true;
    }

    public void Disable()
    {
      foreach (var lifetime in _lifetimes)
      {
        lifetime.EndLife();
      }

      _lifetimes.Clear();
      _isEnabled.Value = false;
    }

    private void OnOwningCardJoinedBattlefield(object sender, EventArgs eventArgs)
    {
      if (!_enabledInAllZones)
        Enable();
    }

    private void OnOwningCardLeftBattlefield(object sender, EventArgs eventArgs)
    {
      if (!_enabledInAllZones)
        Disable();
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
      _lifetimes.Initialize(game.ChangeTracker);
      _isEnabled.Initialize(game.ChangeTracker);

      if (_enabledInAllZones)
        Enable();

      Subscribe(this);
      SubscribeToEvents();
    }

    public void Receive(ControllerChanged message)
    {
      if (message.Card == OwningCard)
      {
        Disable();
        Enable();
      }
    }
  }
}