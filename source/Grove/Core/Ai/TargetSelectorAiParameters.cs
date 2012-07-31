namespace Grove.Core.Ai
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Targeting;
  using Zones;

  public delegate List<Targets> TargetSelectorAiDelegate(TargetSelectorAiParameters parameters);

  public class TargetSelectorAiParameters
  {
    public TargetSelectorAiParameters(TargetsCandidates candidates, TargetSelector selector, Card source,
      int? maxX, bool forceOne, Game game)
    {
      AllCandidates = candidates;
      Selector = selector;
      Source = source;
      MaxX = maxX;
      IsTriggeredAbilityTarget = forceOne;
      Game = game;
    }

    public Game Game { get; private set; }
    public TargetsCandidates AllCandidates { get; private set; }
    public TargetSelector Selector { get; private set; }
    public Card Source { get; private set; }
    public int? MaxX { get; private set; }
    public bool IsTriggeredAbilityTarget { get; private set; }
    public Player Opponent { get { return Game.Players.GetOpponent(Source.Controller); } }
    public Player Controller { get { return Source.Controller; } }
    public Combat Combat { get { return Game.Combat; } }
    public Step Step { get { return Game.Turn.Step; } }
    public Stack Stack { get { return Game.Stack; } }


    public IList<TargetCandidates> EffectCandidates { get { return AllCandidates.Effect; } }

    public IEnumerable<ITarget> Candidates()
    {
      return AllCandidates.HasCost ? AllCandidates.Cost[0] : AllCandidates.Effect[0];
    }

    public List<Targets> MultipleTargets(IDamageDistributor distributor, IList<ITarget> choice)
    {
      var targets = new Targets();

      foreach (var candidate in choice)
      {
        targets.AddEffect(candidate);
      }

      targets.DamageDistributor = distributor;
      return targets.ToEnumerable().ToList();
    }

    public List<Targets> MultipleTargets(params IList<ITarget>[] choices)
    {
      return MultipleTargets((IList<IList<ITarget>>) choices);
    }

    public List<Targets> MultipleTargets(IList<IList<Card>> choices)
    {
      var targetsList = new List<Targets>();

      for (var i = 0; i < choices[0].Count; i++)
      {
        var targets = new Targets();

        for (var j = 0; j < choices.Count; j++)
        {
          targets.AddEffect(choices[j][i]);
        }

        targetsList.Add(targets);
      }
      return targetsList;
    }

    public List<Targets> MultipleTargets(IList<IList<ITarget>> choices)
    {
      var targetsList = new List<Targets>();

      for (var i = 0; i < choices[0].Count; i++)
      {
        var targets = new Targets();

        for (var j = 0; j < choices.Count; j++)
        {
          targets.AddEffect(choices[j][i]);
        }

        targetsList.Add(targets);
      }
      return targetsList;
    }

    public List<Targets> NoTargets()
    {
      return new List<Targets>();
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