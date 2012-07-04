namespace Grove.Core.Ai
{
  using System.Collections.Generic;
  using System.Linq;
  using Zones;

  public delegate List<Targets> TargetsFilterDelegate(TargetFilterParameters parameters);

  public class TargetFilterParameters
  {
    public TargetFilterParameters(TargetsCandidates candidates, Card source, int? maxX, bool forceOne, Game game)
    {
      AllCandidates = candidates;
      Source = source;
      MaxX = maxX;
      ForceOne = forceOne;
      Game = game;
    }

    public Game Game { get; private set; }
    public TargetsCandidates AllCandidates { get; private set; }
    public Card Source { get; private set; }
    public int? MaxX { get; private set; }
    public bool ForceOne { get; private set; }
    public Player Opponent { get { return Game.Players.GetOpponent(Source.Controller); } }
    public Player Controller { get { return Source.Controller; } }
    public Combat Combat { get { return Game.Combat; } }
    public Step Step { get { return Game.Turn.Step; } }
    public Stack Stack { get { return Game.Stack; } }

    public IEnumerable<ITarget> Candidates()
    {
      return AllCandidates.HasCost ? AllCandidates.Cost(0) : AllCandidates.Effect(0);
    }

    public List<Targets> Targets(IEnumerable<ITarget> candidates)
    {
      var targets = AllCandidates.HasCost 
        ? candidates.Select(x => new Targets().AddCost(x)) 
        : candidates.Select(x => new Targets().AddEffect(x));

      return targets.ToList();
    }
  }
}