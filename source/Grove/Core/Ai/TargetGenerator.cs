namespace Grove.Core.Ai
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;

  public class TargetGenerator : IEnumerable<Targets>
  {
    private readonly bool _forceOne;
    private readonly Game _game;
    private readonly int? _maxX;
    private readonly TargetSelector _selector;
    private readonly Card _source;

    private readonly List<Targets> _targets;

    public TargetGenerator(TargetSelector selector, Card source, Game game, int? maxX, bool forceOne = false)
    {
      _selector = selector;
      _source = source;

      _game = game;
      _maxX = maxX;
      _forceOne = forceOne;

      _targets = GetValidTargets();
    }

    public IEnumerator<Targets> GetEnumerator()
    {
      return _targets.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    private IEnumerable<ITarget> GenerateTargets()
    {
      foreach (var target in _game.Players.SelectMany(p => p.GetTargets()))
      {
        yield return target;
      }

      foreach (var target in _game.Stack)
      {
        yield return target;
      }
    }

    private List<Targets> GetValidTargets()
    {            
      var candidates = _selector.GenerateCandidates(GenerateTargets);
      
      var parameters = new SelectorParameters(        
        candidates, 
        _selector,
        _source, 
        _maxX, 
        _forceOne, 
        _game);
      
      return _selector.SelectAiTargets(parameters);
    }
  }
}