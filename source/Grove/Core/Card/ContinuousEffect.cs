namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Events;
  using Infrastructure;
  using Modifiers;

  [Copyable]
  public class ContinuousEffect : GameObject, IReceive<ZoneChangedEvent>, IReceive<PermanentModifiedEvent>,
    ICopyContributor, IDisposable
  {
    public delegate bool CardSelector(Card card, Context ctx);

    private readonly CardSelector _selector;
    private readonly Trackable<IModifier> _doNotUpdate = new Trackable<IModifier>(); // prevent update cycles
    private readonly Trackable<bool> _isActive = new Trackable<bool>();
    private readonly List<CardModifierFactory> _modifierFactories;
    private readonly TrackableList<IModifier> _modifiers = new TrackableList<IModifier>();
    private readonly bool _applyOnlyToPermanents;

    private ContinuousEffect() {}

    public ContinuousEffect(ContinuousEffectParameters p)
    {
      _modifierFactories = p.Modifiers;
      _selector = p.Selector;
      _applyOnlyToPermanents = p.ApplyOnlyToPermanents;
    }
    
    public Player Owner { get; private set; }
    public Card Source { get; private set; }
    public Effect SourceEffect { get; private set; }

    public void AfterMemberCopy(object original)
    {
      SubscribeToEvents();
    }

    public void Receive(PermanentModifiedEvent message)
    {
      if (ShouldPermanentBeUpdated(message.Card, message.Modifier))
      {
        UpdatePermanent(message.Card);
      }
    }

    public void Receive(ZoneChangedEvent message)
    {
      if (_isActive == false) return;
      if (_applyOnlyToPermanents == false) return;

      if (message.ToBattlefield && _selector(message.Card, Ctx))
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
      }
    }

    public void Initialize(Card source, Game game, Player owner, Effect sourceEffect)
    {
      Game = game;

      _doNotUpdate.Initialize(ChangeTracker);
      _modifiers.Initialize(ChangeTracker);
      _isActive.Initialize(ChangeTracker);

      Source = source;
      SourceEffect = sourceEffect;

      Subscribe(this);
      SubscribeToEvents();
    }

    private Context Ctx { get { return new Context(this, Game); } }

    private void SubscribeToEvents()
    {
      Source.JoinedBattlefield += Activate;
      Source.LeftBattlefield += Deactivate;
    }

    private bool ShouldPermanentBeUpdated(Card permanent, IModifier modifier)
    {
      return _isActive && permanent.Zone == Zone.Battlefield && modifier != _doNotUpdate.Value;
    }

    private void UpdatePermanent(Card permanent)
    {
      var modifier = FindModifier(permanent);
      var shouldEffectBeApliedToPermanent = _selector(permanent, Ctx);

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

    private void RemoveModifier(IModifier modifier)
    {
      _doNotUpdate.Value = modifier;

      _modifiers.Remove(modifier);
      modifier.Owner.RemoveModifier(modifier);
    }

    private IModifier FindModifier(Card permanent)
    {
      return _modifiers
        .FirstOrDefault(x => x.Owner == permanent);
    }

    public void Activate()
    {
      var cards = _applyOnlyToPermanents
        ? Players.Permanents()
        : Players.AllCards();

      foreach (var card in cards.Where(x => _selector(x, Ctx)))
      {
        AddModifier(card);
      }

      _isActive.Value = true;
    }


    private void AddModifier(Card card)
    {
      foreach (var factory in _modifierFactories)
      {
        var modifier = factory();

        _modifiers.Add(modifier);
        _doNotUpdate.Value = modifier;

        var p = new ModifierParameters
          {
            SourceCard = Source,
          };

        card.AddModifier(modifier, p);
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

    public void Dispose()
    {
      Source.JoinedBattlefield -= Activate;
      Source.LeftBattlefield -= Deactivate;
    }

    public class Context
    {
      private readonly ContinuousEffect _effect;
      private readonly Game _game;

      public Context(ContinuousEffect effect, Game game)
      {
        _effect = effect;
        _game = game;
      }
      
      // this should matter only when using emblems
      public Player EffectOwner { get { return _effect.Owner; } }
      public Player You { get { return _effect.Source.Controller; } }
      public Card Source { get { return _effect.Source; } }      
      public Player Opponent { get { return You.Opponent; } }
    }
  }
}