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
  public class ContinuousEffect : IReceive<CardChangedZone>, ILifetimeDependency
  {
    public delegate bool ShouldApplyToCard(Card card, Card effectSource);

    public delegate bool ShouldApplyToPlayer(Player player, Card effectSource);

    private readonly ChangeTracker _changeTracker;

    public ShouldApplyToCard CardFilter = delegate { return true; };
    public ShouldApplyToPlayer PlayerFilter = delegate { return false; };

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

      if (CanAddModifierToCard(message))
      {
        AddModifierToTarget(message.Card);
      }
    }

    private void Activate()
    {
      AddModifierToPermanents();
      AddModifierToPlayers();
      _isActive.Value = true;
    }

    private void AddModifierToPlayers()
    {
      foreach (var player in _players)
      {
        if (PlayerFilter(player, _source))
        {
          AddModifierToTarget(player);
        }
      }
    }

    private void AddModifierToTarget(ITarget target)
    {
      var modifier = ModifierFactory.CreateModifier(_source, target);
      modifier.AddLifetime(new DependantLifetime(this, _changeTracker));
      target.AddModifier(modifier);
    }

    private void AddModifierToPermanents()
    {
      var permanents = _players.Permanents()
        .Where(permanent => CardFilter(permanent, _source)).ToList();

      foreach (var permanent in permanents)
      {
        AddModifierToTarget(permanent);
      }
    }

    private bool CanAddModifierToCard(CardChangedZone message)
    {
      return message.ToBattlefield && CardFilter(message.Card, _source);
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

        Init(continuousEffect, new CardBuilder(Game));

        Game.Publisher.Subscribe(continuousEffect);
        return continuousEffect;
      }
    }
  }
}