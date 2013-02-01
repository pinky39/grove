namespace Grove.Core
{
  using System;
  using Ai;
  using Decisions;
  using Dsl;
  using Infrastructure;
  using Zones;

  [Copyable]
  public abstract class GameObject
  {
    protected Game Game { get; set; }
    
    protected Players Players {get { return Game.Players; }}
    protected Stack Stack { get { return Game.Stack; } }
    protected Combat Combat {get { return Game.Combat; }}    
    protected TurnInfo Turn {get { return Game.Turn; }}
    protected Search Search {get { return Game.Search; }}

    protected Dsl.CardBuilder Builder { get { return new Dsl.CardBuilder(); } }
    protected ChangeTracker ChangeTracker {get { return Game.ChangeTracker; }}
    
    protected void Publish<T>(T message)
    {
      Game.Publish(message);
    }       

    protected void Unsubscribe(object obj = null)
    {
      obj = obj ?? this;
      Game.Unsubscribe(obj);
    }

    protected void Subscribe(object obj = null)
    {
      obj = obj ?? this;
      Game.Subscribe(obj);
    }

    protected void Enqueue<TDecision>(Player controller, Action<TDecision> setParameters) where TDecision : class, IDecision
    {
      Game.Enqueue(controller, setParameters);
    }
  }
}