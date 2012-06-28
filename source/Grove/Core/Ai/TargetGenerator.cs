namespace Grove.Core.Ai
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;

  public class TargetGenerator : IEnumerable<Targets>
  {    
    private readonly bool _forceOne;
    private readonly Game _game;
    private readonly int? _maxX;
    private readonly TargetSelectors _selectors;

    private readonly List<Targets> _targets;

    public TargetGenerator(TargetSelectors selectors, Game game,
                           int? maxX, bool forceOne = false)
    {
      _selectors = selectors;
      
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
      var candidates = _selectors.GenerateCandidates(GenerateTargets);
      return _selectors.Filter(new TargetPickerParameters(candidates, _maxX, _forceOne, _game));
    }
  }
}