namespace Grove.Triggers
{
  using System;
  using Infrastructure;

  public abstract class Trigger : GameObject, IHashable
  {
    public delegate bool Predicate(Context ctx);

    public delegate bool CardSelector(Card card, Context ctx);

    public Predicate Condition = delegate { return true; };
    public TriggeredAbility Ability { get; private set; }
    public Card OwningCard { get { return Ability.OwningCard; } }
    public Player Controller { get { return Ability.OwningCard.Controller; } }

    public int CalculateHash(HashCalculator calc)
    {
      return GetType().GetHashCode();
    }

    public void Deactivate()
    {
      Unsubscribe(this);
    }

    public void Activate()
    {
      Subscribe(this);
      OnActivate();
    }

    public event EventHandler<TriggerEventArgs> Triggered = delegate { };

    protected void Set(object context = null)
    {
      if (Condition(new Context(this, Game)))
        Triggered(this, new TriggerEventArgs(context));
    }

    public virtual void Initialize(TriggeredAbility triggeredAbility, Game game)
    {
      Game = game;
      Ability = triggeredAbility;

      Initialize();
    }

    protected virtual void Initialize() {}
    protected virtual void OnActivate() {}

    public class TriggerEventArgs : EventArgs
    {
      public TriggerEventArgs(object triggerMessage)
      {
        TriggerMessage = triggerMessage;
      }

      public object TriggerMessage { get; private set; }
    }

    public class Context
    {
      private readonly Trigger _trigger;
      private readonly Game _game;

      public Context(Trigger trigger, Game game)
      {
        _trigger = trigger;
        _game = game;
      }

      public Card OwningCard {get { return _trigger.OwningCard; }}
      public Player You { get { return _trigger.Controller; } }
      public Player Opponent { get { return _trigger.Controller.Opponent; } }
      public TurnInfo Turn { get { return _game.Turn; } }
      public Players Players { get { return _game.Players; } }
    }
  }
}