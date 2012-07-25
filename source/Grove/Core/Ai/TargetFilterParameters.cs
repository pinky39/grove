namespace Grove.Core.Ai
{
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;
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
    public IPlayer Opponent { get { return Game.Players.GetOpponent(Source.Controller); } }
    public ICardController Controller { get { return Source.Controller; } }
    public Combat Combat { get { return Game.Combat; } }
    public Step Step { get { return Game.Turn.Step; } }
    public Stack Stack { get { return Game.Stack; } }

    public IEnumerable<Target> Candidates(int index = 0)
    {
      return AllCandidates.HasCost ? AllCandidates.Cost(index) : AllCandidates.Effect(index);
    }

    public List<Targets> MultipleTargets(params List<Target>[] candidates)
    {
      var targetsList = new List<Targets>();

      for (var i = 0; i < candidates[0].Count; i++)
      {
        var targets = new Targets();

        for (var j = 0; j < candidates.Length; j++)
        {
          targets.AddEffect(candidates[j][i]);
        }

        targetsList.Add(targets);
      }
      return targetsList;
    }

    public List<Targets> NoTargets()
    {
      return new List<Targets>();
    }
    
    public List<Targets> Targets(IEnumerable<Card> candidates)
    {
      return Targets(candidates.Select(x => new Target(x)));
    }
        
    public List<Targets> Targets(IEnumerable<Target> candidates)
    {
      var targets = AllCandidates.HasCost
        ? candidates.Select(x => new Targets().AddCost(x))
        : candidates.Select(x => new Targets().AddEffect(x));

      return targets.ToList();
    }
  }
}