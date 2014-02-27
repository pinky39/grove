namespace Grove.Triggers
{
  using System;
  using Grove.Infrastructure;

  public abstract class Trigger : GameObject, IHashable
  {
    public Func<Trigger, Game, bool> Condition = delegate { return true; };
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
      if (Condition(this, Game))
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
  }
}