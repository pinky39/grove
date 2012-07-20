namespace Grove.Core.Details.Cards.Triggers
{
  using System;
  using Dsl;
  using Infrastructure;

  [Copyable]
  public abstract class Trigger : IDisposable, IHashable
  {
    public Func<Trigger, bool> Condition = delegate { return true; };
    
    protected TriggeredAbility Ability { get; private set; }
    public Game Game { get; private set; }
    public Card OwningCard { get { return Ability.OwningCard; } }
    protected IPlayer Controller { get { return Ability.OwningCard.Controller; } }
    protected Players Players { get { return Game.Players; } }
    protected Publisher Publisher { get { return Game.Publisher; } }

    private Trackable<bool> _canTrigger;
    protected bool CanTrigger { get { return _canTrigger.Value; } set { _canTrigger.Value = value; } }

    public void Dispose()
    {
      Publisher.Unsubscribe(this);
    }

    public int CalculateHash(HashCalculator calc)
    {
      return GetType().GetHashCode();
    }

    public event EventHandler<TriggerEventArgs> Triggered = delegate { };

    protected void Set(object context = null)
    {
      if (CanTrigger && Condition(this))
        Triggered(this, new TriggerEventArgs(context));
    }

    protected virtual void Initialize() {}

    [Copyable]
    public class Factory<TTrigger> : ITriggerFactory where TTrigger : Trigger, new()
    {
      public Initializer<TTrigger> Init = delegate { };
      public Game Game { get; set; }

      public Trigger CreateTrigger(TriggeredAbility triggeredAbility)
      {
        var trigger = new TTrigger();
        trigger.Ability = triggeredAbility;
        trigger.Game = Game;
        trigger._canTrigger = new Trackable<bool>(true, Game.ChangeTracker);

        Init(trigger, new CardBuilder(Game));
        trigger.Initialize();

        Game.Publisher.Subscribe(trigger);

        return trigger;
      }
    }

    public class TriggerEventArgs : EventArgs
    {
      public TriggerEventArgs(object context)
      {
        Context = context;
      }

      public object Context { get; private set; }
    }
  }
}