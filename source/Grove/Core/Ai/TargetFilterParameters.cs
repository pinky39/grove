namespace Grove.Core.Ai
{
  using System.Collections.Generic;
  using System.Linq;

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

    public IEnumerable<ITarget> Candidates(TargetFilters.InputSelectorDelegate selector = null)
    {
      return selector == null ? AllCandidates.Effect(0) : selector(AllCandidates);
    }

    public List<Targets> Targets(IEnumerable<ITarget> candidates, TargetFilters.OutputSelectorDelegate selector = null)
    {
      return (selector == null ? candidates.Select(x => new Targets().AddEffect(x)) : selector(candidates)).ToList();
    }
  }
}