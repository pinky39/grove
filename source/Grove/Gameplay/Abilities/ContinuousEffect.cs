namespace Grove.Gameplay.Abilities
{
  using System.Linq;
  using Effects;
  using Infrastructure;
  using Messages;
  using Misc;
  using Modifiers;
  using Zones;

  public delegate bool ShouldApplyToCard(Card card, ContinuousEffect effect);

  public delegate bool ShouldApplyToPlayer(Player player, ContinuousEffect effect);

  [Copyable]
  public class ContinuousEffect : GameObject, IReceive<ZoneChanged>, IReceive<PermanentWasModified>, ICopyContributor
  {
    private readonly ShouldApplyToCard _cardFilter;
    private readonly Trackable<IModifier> _doNotUpdate = new Trackable<IModifier>();
    private readonly Trackable<bool> _isActive = new Trackable<bool>();
    private readonly CardModifierFactory _modifierFactory;
    private readonly TrackableList<IModifier> _modifiers = new TrackableList<IModifier>();

    private ContinuousEffect() {}

    public ContinuousEffect(ContinuousEffectParameters p)
    {
      _modifierFactory = p.Modifier;
      _cardFilter = p.CardFilter;
    }

    public Card Source { get; private set; }
    public Effect SourceEffect { get; private set; }

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
      ApplyModifierToPermanents();
      _isActive.Value = true;
    }


    private void AddModifier(Card card)
    {
      var modifier = _modifierFactory();

      _modifiers.Add(modifier);
      _doNotUpdate.Value = modifier;

      var p = new ModifierParameters
        {
          SourceCard = Source,
        };

      card.AddModifier(modifier, p);
    }

    private void ApplyModifierToPermanents()
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