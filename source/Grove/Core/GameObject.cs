namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using Decisions;
  using Events;
  using Infrastructure;

  [Copyable]
  public abstract class GameObject
  {
    public bool IsInitialized
    {
      get { return Game != null; }
    }

    protected Game Game { get; set; }

    protected Players Players
    {
      get { return Game.Players; }
    }

    protected Stack Stack
    {
      get { return Game.Stack; }
    }

    protected Combat Combat
    {
      get { return Game.Combat; }
    }

    protected TurnInfo Turn
    {
      get { return Game.Turn; }
    }

    protected SearchRunner Ai
    {
      get { return Game.Ai; }
    }

    protected ChangeTracker ChangeTracker
    {
      get { return Game.ChangeTracker; }
    }

    public void SaveDecisionResult(object result)
    {
      Game.Recorder.SaveDecisionResult(result);
    }

    public List<ITarget> GenerateTargets(Func<Zone, Player, bool> zoneFilter)
    {
      var targets = new List<ITarget>();

      Players.Player1.GetTargets(zoneFilter, targets);
      Players.Player2.GetTargets(zoneFilter, targets);
      Stack.GenerateTargets(zoneFilter, targets);

      return targets;
    }

    protected CardColor GetMostCommonColor(IEnumerable<Card> cards)
    {
      var counts = new Dictionary<CardColor, int>
        {
          {CardColor.White, 0},
          {CardColor.Blue, 0},
          {CardColor.Black, 0},
          {CardColor.Red, 0},
          {CardColor.Green, 0},
          {CardColor.Colorless, -100},
          {CardColor.None, -100},
        };

      foreach (var color in cards.SelectMany(card => card.Colors))
      {
        counts[color]++;
      }

      return counts.MaxElement(x => x.Value).Key;
    }

    protected int GenerateRandomNumber(int minValue, int maxValue)
    {
      return Game.Random.Next(minValue, maxValue);
    }

    protected IList<int> GetRandomPermutation(int start, int count)
    {
      return Game.Random.GetRandomPermutation(start, count);
    }

    protected bool FlipACoin(Player who)
    {
      // in search always consider winning the coin flip
      var hasWon = Ai.IsSearchInProgress || Game.Random.FlipACoin();
      Publish(new PlayerFlippedCoinEvent(who, hasWon));

      return hasWon;
    }

    protected int RollADice(int numOfSides = 42)
    {            
      return Game.Random.RollADice(numOfSides);
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

    protected void Enqueue(Decision decision)
    {
      Game.Enqueue(decision);
    }

    // Decisions are normally executed seperataly
    // by the state machine to support generating
    // multiple branches in a search tree.
    // For this to work the code must be written 
    // in a certain way. (It must be broken in 
    // many small pieces so the statemachine
    // can resume execution.)
    // 
    // For non-branching decisions we can cheat
    // and execute decisions immediately, this
    // simplifies the code where branching is
    // not needed.
    protected TResult Execute<TResult>(Decision decision)
    {
      var handler = decision.CreateHandler(Game);
      handler.Execute();
      return (TResult)handler.Result;
    }
  }
}