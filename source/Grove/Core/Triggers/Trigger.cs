namespace Grove.Core.Triggers
{
  using System;
  using Infrastructure;

  public abstract class Trigger : GameObject, IDisposable, IHashable
  {
    public Func<Trigger, bool> Condition = delegate { return true; };
    
    private readonly Trackable<bool> _canTrigger = new Trackable<bool>(true);
    protected TriggeredAbility Ability { get; private set; }
    public Card OwningCard { get { return Ability.OwningCard; } }
    protected Player Controller { get { return Ability.OwningCard.Controller; } }
    protected bool CanTrigger { get { return _canTrigger.Value; } set { _canTrigger.Value = value; } }

    public void Dispose()
    {
      Unsubscribe(this);
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

    public virtual void Initialize(TriggeredAbility triggeredAbility, Game game)
    {
      Game = game;
      Ability = triggeredAbility;

      _canTrigger.Initialize(game.ChangeTracker);      
      Subscribe(this);
    }

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