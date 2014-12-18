namespace Grove
{
  using System.Collections.Generic;
  using System.Linq;
  using Events;
  using Infrastructure;
  using Modifiers;

  public delegate bool ShouldApplyToCard(Card card, ContinuousEffect effect);

  public delegate bool ShouldApplyToPlayer(Player player, ContinuousEffect effect);

  [Copyable]
  public class ContinuousEffect : GameObject, IReceive<ZoneChangedEvent>, IReceive<PermanentModifiedEvent>,
    ICopyContributor
  {
    private readonly ShouldApplyToCard _cardFilter;
    private readonly Trackable<IModifier> _doNotUpdate = new Trackable<IModifier>(); // prevent update cycles
    private readonly Trackable<bool> _isActive = new Trackable<bool>();
    private readonly List<CardModifierFactory> _modifierFactories;
    private readonly TrackableList<IModifier> _modifiers = new TrackableList<IModifier>();
    private readonly bool _applyOnlyToPermaments;

    private ContinuousEffect() {}

    public ContinuousEffect(ContinuousEffectParameters p)
    {
      _modifierFactories = p.Modifiers;
      _cardFilter = p.CardFilter;
      _applyOnlyToPermaments = p.ApplyOnlyToPermaments;
    }

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
      if (_applyOnlyToPermaments == false) return;

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
      }
    }

    public void Initialize(Card source, Game game, Effect sourceEffect = null)
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
      var cards = _applyOnlyToPermaments
        ? Players.Permanents()
        : Players.AllCards();

      foreach (var card in cards.Where(x => _cardFilter(x, this)))
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
  }
}