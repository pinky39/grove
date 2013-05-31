namespace Grove.Gameplay.Misc
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Artifical;
  using Decisions;
  using Infrastructure;
  using States;
  using Targeting;
  using Zones;

  [Copyable]
  public abstract class GameObject
  {
    public bool IsInitialized { get { return Game != null; } }
    protected Game Game { get; set; }

    protected Coin Coin { get { return Game.Coin; } }
    protected Dice Dice { get { return Game.Dice; } }    
    protected Players Players { get { return Game.Players; } }
    protected Stack Stack { get { return Game.Stack; } }
    protected Combat Combat { get { return Game.Combat; } }
    protected TurnInfo Turn { get { return Game.Turn; } }
    protected SearchRunner Ai { get { return Game.Ai; } }
    protected CardsDatabase CardsDatabase { get { return Game.CardsDatabase; } }

    protected ChangeTracker ChangeTracker { get { return Game.ChangeTracker; } }

    protected int GenerateRandomNumber(int minValue, int maxValue)
    {
      return Game.Random.Next(minValue, maxValue);
    }

    protected IList<int> GetRandomPermutation(int start, int count)
    {
      return Game.Random.GetRandomPermutation(start, count);
    }

    public void SaveDecisionResult(object result)
    {
      Game.SaveDecisionResult(result);
    }

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

    protected void Enqueue<TDecision>(Player controller, Action<TDecision> init = null)
      where TDecision : class, IDecision
    {
      Game.Enqueue(controller, init);
    }

    public IEnumerable<ITarget> GenerateTargets(Func<Zone, Player, bool> zoneFilter)
    {
      foreach (var target in Players.SelectMany(p => p.GetTargets(zoneFilter)))
      {
        yield return target;
      }

      foreach (var target in Stack.GenerateTargets(zoneFilter))
      {
        yield return target;
      }
    }
  }
}