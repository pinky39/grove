namespace Grove.Core.Modifiers
{
  using System.Linq;
  using CardDsl;
  using Infrastructure;
  using Messages;
  using Zones;

  [Copyable]
  public class ContinuousEffect : IReceive<CardChangedZone>, ILifetimeDependency
  {
    public delegate bool ShouldApplyToCard(Card card, Card effectSource);

    private readonly ChangeTracker _changeTracker;

    public ShouldApplyToCard Filter = delegate { return true; };
    private Trackable<bool> _isActive;
    private Players _players;
    private Card _source;

    private ContinuousEffect()
    {
      /* for state copy */
    }

    public ContinuousEffect(ChangeTracker changeTracker)
    {
      _changeTracker = changeTracker;
      EndOfLife = new TrackableEvent(this, changeTracker);
    }

    public IModifierFactory ModifierFactory { get; set; }
    public TrackableEvent EndOfLife { get; set; }

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

      if (CanAddModifier(message))
      {
        AddModifier(message.Card);
      }
    }

    private void Activate()
    {
      AddModifierToPermanents();
      _isActive.Value = true;
    }

    private void AddModifier(Card permanent)
    {
      var modifier = ModifierFactory.CreateModifier(_source, permanent);
      modifier.AddLifetime(new DependantLifetime(modifier, this, _changeTracker));
      permanent.AddModifier(modifier);
    }

    private void AddModifierToPermanents()
    {
      var permanents = _players.Permanents()
        .Where(permanent => Filter(permanent, _source)).ToList();

      foreach (var permanent in permanents)
      {
        AddModifier(permanent);
      }
    }

    private bool CanAddModifier(CardChangedZone message)
    {
      return message.ToBattlefield && Filter(message.Card, _source);
    }

    private void Deactivate()
    {
      EndOfLife.Raise();
      _isActive.Value = false;
    }

    private bool SourceJoinedBattlefield(CardChangedZone message)
    {
      return message.Card == _source && message.To == Zone.Battlefield;
    }

    private bool SourceLeftBattlefield(CardChangedZone message)
    {
      return message.Card == _source && message.From == Zone.Battlefield;
    }

    [Copyable]
    public class Factory : IContinuousEffectFactory
    {
      public Initializer<ContinuousEffect> Init = delegate { };
      public Game Game { get; set; }

      public ContinuousEffect Create(Card source)
      {
        var continuousEffect = new ContinuousEffect(Game.ChangeTracker);
        continuousEffect._source = source;
        continuousEffect._players = Game.Players;
        continuousEffect._isActive = new Trackable<bool>(Game.ChangeTracker);

        Init(continuousEffect, new CardCreationContext(Game));

        Game.Publisher.Subscribe(continuousEffect);
        return continuousEffect;
      }
    }
  }
}