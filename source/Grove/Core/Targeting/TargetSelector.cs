namespace Grove.Core.Targeting
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Ai;
  using Infrastructure;
  using Core.Zones;

  public delegate IEnumerable<ITarget> TargetGeneratorDelegate(Func<Zone, Player, bool> zoneFilter);

  [Copyable]
  public class TargetSelector
  {
    private readonly List<TargetValidator> _costValidators = new List<TargetValidator>();
    private readonly List<TargetValidator> _effectValidators = new List<TargetValidator>();

    public static readonly TargetSelector NullSelector = new TargetSelector();
    
    public TargetSelector(IEnumerable<TargetValidator> effectValidators,
      IEnumerable<TargetValidator> costValidators, TargetingAiDelegate aiSelector)
    {
      _effectValidators.AddRange(effectValidators);
      _costValidators.AddRange(costValidators);
      AiSelector = aiSelector;
    }
    
    private TargetSelector() {}

    public TargetingAiDelegate AiSelector { get; private set; }

    public int Count { get { return _costValidators.Count + _effectValidators.Count; } }
    
    public bool RequiresTargets {get { return _costValidators.Any() || _effectValidators.Any(); }}
    public bool RequiresCostTargets { get { return _costValidators.Count > 0; } }    
    public bool RequiresEffectTargets { get { return _effectValidators.Count > 0; } }

    public IList<TargetValidator> Effect { get { return _effectValidators; } }
    public IList<TargetValidator> Cost { get { return _costValidators; } }

    public int GetMinEffectTargetCount()
    {
      return Effect.Sum(x => x.MinCount);
    }

    public int GetMaxEffectTargetCount()
    {
      return Effect.Sum(x => x.MaxCount).Value;

    }

    public TargetsCandidates GenerateCandidates(TargetGeneratorDelegate generator)
    {
      var all = new TargetsCandidates();

      foreach (var selector in _costValidators)
      {
        var candidates = new TargetCandidates();

        foreach (var target in generator(selector.IsValidZone))
        {
          if (selector.IsValid(target))
          {
            candidates.Add(target);
          }
        }

        all.AddCostCandidates(candidates);
      }

      foreach (var selector in _effectValidators)
      {
        var candidates = new TargetCandidates();

        foreach (var target in generator(selector.IsValidZone))
        {
          if (selector.IsValid(target))
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
                (zone == null || validator.IsValidZone(zone.Value, controller)) &&
                 validator.IsValid(target);
            });
    }

    public void SetTrigger(object trigger)
    {
      foreach (var targetValidator in Effect)
      {
        targetValidator.Trigger = trigger;
      }
    }
  }
}