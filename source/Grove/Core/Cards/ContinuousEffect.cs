namespace Grove.Core.Cards
{
  using System.Linq;
  using Dsl;
  using Infrastructure;
  using Messages;
  using Modifiers;
  using Targeting;
  using Zones;

  [Copyable]
  public class ContinuousEffect : IReceive<CardChangedZone>, IReceive<PermanentWasModified>
  {
    public delegate bool ShouldApplyToCard(Card card, ContinuousEffect effect);
    public delegate bool ShouldApplyToPlayer(Player player, ContinuousEffect effect);
    
    public ShouldApplyToCard CardFilter = delegate { return false; };
    public ShouldApplyToPlayer PlayerFilter = delegate { return false; };
    
    private Trackable<Modifier> _doNotUpdate;    
    private Game _game;

    private Trackable<bool> _isActive;
    private TrackableList<Modifier> _modifiers;
    public Card Source { get; private set; }
    public ITarget Target { get; private set; }

    private ContinuousEffect()
    {
      /* for state copy */
    }

    public IModifierFactory ModifierFactory { get; set; }

    public void Receive(CardChangedZone message)
    {      
      if (_isActive == false) return;      
      
      if (message.ToBattlefield && CardFilter(message.Card, this))
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

    public void Receive(PermanentWasModified message)
    {
      if (ShouldPermanentBeUpdated(message.Card, message.Modifier))
      {
        UpdatePermanent(message.Card);
      }
    }

    private bool ShouldPermanentBeUpdated(Card permanent, IModifier modifier)
    {
      return _isActive && permanent.Zone == Zone.Battlefield && modifier != _doNotUpdate.Value;
    }

    private void UpdatePermanent(Card permanent)
    {
      var modifier = FindModifier(permanent);
      var shouldEffectBeApliedToPermanent = CardFilter(permanent, this);

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
      foreach (var player in _game.Players)
      {
        if (PlayerFilter(player, this))
        {
          AddModifier(player);
        }
      }
    }

    private void AddModifier(ITarget target)
    {
      var modifier = ModifierFactory.CreateModifier(Source, target, x: null, game: _game);
      _modifiers.Add(modifier);

      _doNotUpdate.Value = modifier;
      target.AddModifier(modifier);
    }

    private void AddModifierToPermanents()
    {
      var permanents = _game.Players.Permanents()
        .Where(permanent => CardFilter(permanent, this)).ToList();

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

    public class Factory : IContinuousEffectFactory
    {
      public Initializer<ContinuousEffect> Init = delegate { };

      public ContinuousEffect Create(Card source, ITarget target, Game game)
      {
        var continuousEffect = new ContinuousEffect();
        continuousEffect._doNotUpdate = new Trackable<Modifier>(game.ChangeTracker);
        continuousEffect._modifiers = new TrackableList<Modifier>(game.ChangeTracker);
        continuousEffect.Source = source;
        continuousEffect.Target = target;
        continuousEffect._game = game;
        continuousEffect._isActive = new Trackable<bool>(game.ChangeTracker);

        Init(continuousEffect);

        game.Subscribe(continuousEffect);
        return continuousEffect;
      }
    }
  }
}