namespace Grove.Core.Targeting
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Core.Zones;
  using Infrastructure;

  public delegate IEnumerable<ITarget> TargetGeneratorDelegate(Func<Zone, Player, bool> zoneFilter);

  [Copyable]
  public class TargetSelector
  {
    public static readonly TargetSelector NullSelector = new TargetSelector();
    private readonly List<TargetValidator> _costValidators = new List<TargetValidator>();
    private readonly List<TargetValidator> _effectValidators = new List<TargetValidator>();

    public int Count { get { return _costValidators.Count + _effectValidators.Count; } }

    public bool RequiresTargets { get { return _costValidators.Any() || _effectValidators.Any(); } }
    public bool RequiresCostTargets { get { return _costValidators.Count > 0; } }
    public bool RequiresEffectTargets { get { return _effectValidators.Count > 0; } }

    public IList<TargetValidator> Effect { get { return _effectValidators; } }
    public IList<TargetValidator> Cost { get { return _costValidators; } }

    public TargetSelector AddEffect(Action<TargetValidatorParameters> set)
    {
      var validator = CreateValidator(set);

      _effectValidators.Add(validator);
      return this;
    }

    private TargetValidator CreateValidator(Action<TargetValidatorParameters> set)
    {
      var p = new TargetValidatorParameters();
      set(p);
      return new TargetValidator(p);
    }

    public TargetSelector AddCost(Action<TargetValidatorParameters> set)
    {
      var validator = CreateValidator(set);

      _costValidators.Add(validator);
      return this;
    }

    public int GetMinEffectTargetCount()
    {
      return Effect.Sum(x => x.MinCount);
    }

    public int GetMaxEffectTargetCount()
    {
      return Effect.Sum(x => x.MaxCount).Value;
    }

    public void Initialize(Game game)
    {
      foreach (var validator in _effectValidators)
      {
        validator.Initialize(game);
      }

      foreach (var validator in _costValidators)
      {
        validator.Initialize(game);
      }
    }

    public TargetsCandidates GenerateCandidates(TargetGeneratorDelegate generator)
    {
      var all = new TargetsCandidates();

      foreach (var selector in _costValidators)
      {
        var candidates = new TargetCandidates();

        foreach (var target in generator(selector.IsZoneValid))
        {
          if (selector.IsTargetValid(target))
          {
            candidates.Add(target);
          }
        }

        all.AddCostCandidates(candidates);
      }

      foreach (var selector in _effectValidators)
      {
        var candidates = new TargetCandidates();

        foreach (var target in generator(selector.IsZoneValid))
        {
          if (selector.IsTargetValid(target))
          {
            candidates.Add(target);
          }
        }

        all.AddEffectCandidates(candidates);
      }

      return all;
    }

    public bool IsValidEffectTarget(ITarget target)
    {
      // Currently there is no way to figure out
      // to which validator the target belongs. 
      // All validators are tried therefore.
      // Currently there are no problems with this, if
      // there are problems in the future this must be 
      // changed, so the target will know to which
      // validator it belongs.            
      var zone = target.Zone();

      return _effectValidators.Any(
        validator =>
          {
            var controller = target.IsCard() ? target.Card().Controller : null;

            return
              (zone == null || validator.IsZoneValid(zone.Value, controller)) &&
                validator.IsTargetValid(target);
          });
    }
  }
}