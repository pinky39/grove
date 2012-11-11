namespace Grove.Core.Details.Cards
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
    public delegate bool ShouldApplyToCard(Card card, Card effectSource);
    public delegate bool ShouldApplyToPlayer(Player player, Card effectSource);

    public ShouldApplyToCard CardFilter = delegate { return false; };
    public ShouldApplyToPlayer PlayerFilter = delegate { return false; };
    private Trackable<Modifier> _doNotUpdate;

    private Trackable<bool> _isActive;
    private TrackableList<Modifier> _modifiers;    
    private Card _source;
    private Game _game;

    private ContinuousEffect()
    {
      /* for state copy */
    }

    public IModifierFactory ModifierFactory { get; set; }

    public void Receive(CardChangedZone message)
    {
      if (SourceJoinedBattlefield(message))
      {
        Activate();
        return;
      }

      if (!_isActive)
      {
        return;
      }

      if (SourceLeftBattlefield(message))
      {
        Deactivate();
        return;
      }

      if (message.ToBattlefield && CardFilter(message.Card, _source))
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
      var shouldEffectBeApliedToPermanent = CardFilter(permanent, _source);

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

    private void Activate()
    {
      AddModifierToPermanents();
      AddModifierToPlayers();
      _isActive.Value = true;
    }

    private void AddModifierToPlayers()
    {
      foreach (var player in _game.Players)
      {
        if (PlayerFilter(player, _source))
        {
          AddModifier(player);
        }
      }
    }

    private void AddModifier(ITarget target)
    {
      var modifier = ModifierFactory.CreateModifier(_source, target, x: null, game: _game);
      _modifiers.Add(modifier);

      _doNotUpdate.Value = modifier;
      target.AddModifier(modifier);
    }

    private void AddModifierToPermanents()
    {
      var permanents = _game.Players.Permanents()
        .Where(permanent => CardFilter(permanent, _source)).ToList();

      foreach (var permanent in permanents)
      {
        AddModifier(permanent);
      }
    }

    private void Deactivate()
    {
      _isActive.Value = false;
      
      foreach (var modifier in _modifiers.ToList())
      {
        RemoveModifier(modifier);
      }      
    }

    private bool SourceJoinedBattlefield(CardChangedZone message)
    {
      return message.Card == _source && message.To == Zone.Battlefield;
    }

    private bool SourceLeftBattlefield(CardChangedZone message)
    {
      return message.Card == _source && message.From == Zone.Battlefield;
    }
    
    public class Factory : IContinuousEffectFactory
    {
      public Initializer<ContinuousEffect> Init = delegate { };      

      public ContinuousEffect Create(Card source, Game game)
      {
        var continuousEffect = new ContinuousEffect();
        continuousEffect._doNotUpdate = new Trackable<Modifier>(game.ChangeTracker);
        continuousEffect._modifiers = new TrackableList<Modifier>(game.ChangeTracker);
        continuousEffect._source = source;        
        continuousEffect._game = game;
        continuousEffect._isActive = new Trackable<bool>(game.ChangeTracker);

        Init(continuousEffect);

        game.Subscribe(continuousEffect);
        return continuousEffect;
      }
    }
  }
}