namespace Grove.Gameplay.Targeting
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Card;
  using Common;

  public class TargetSelector : GameObject
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
      var org = set;

      set = p =>
        {
          org(p);
          p.MustBeTargetable = false;
        };

      var validator = CreateValidator(set);
      _costValidators.Add(validator);
      return this;
    }

    public int GetMinTargetCount(int? x)
    {
      return Effect.Count > 0 ? 
        Effect.Sum(y => y.MinCount.GetValue(x)) : 
        Cost.Sum(y => y.MinCount.GetValue(x));
    }

    public int GetMaxTargetCount(int? x)
    {
      return Effect.Count > 0 ? 
        Effect.Sum(y => y.MaxCount.GetValue(x)) : 
        Cost.Sum(y => y.MaxCount.GetValue(x));
    }

    public void Initialize(Card owningCard, Game game)
    {
      Game = game;

      foreach (var validator in _effectValidators)
      {
        validator.Initialize(game, owningCard.Controller, owningCard);
      }

      foreach (var validator in _costValidators)
      {
        validator.Initialize(game, owningCard.Controller, owningCard);
      }
    }

    public TargetsCandidates GenerateCandidates(object triggerMessage = null)
    {
      var all = new TargetsCandidates();

      foreach (var selector in _costValidators)
      {
        var candidates = new TargetCandidates();

        foreach (var target in GenerateTargets(selector.IsZoneValid))
        {
          if (selector.IsTargetValid(target, triggerMessage))
          {
            candidates.Add(target);
          }
        }

        all.AddCostCandidates(candidates);
      }

      foreach (var selector in _effectValidators)
      {
        var candidates = new TargetCandidates();

        foreach (var target in GenerateTargets(selector.IsZoneValid))
        {
          if (selector.IsTargetValid(target, triggerMessage))
          {
            candidates.Add(target);
          }
        }

        all.AddEffectCandidates(candidates);
      }

      return all;
    }

    public bool IsValidEffectTarget(ITarget target, object triggerMessage = null)
    {
      // Currently there is no way to figure out
      // to which validator the target belongs. 
      // All validators are tried therefore.
      // Currently there are no problems with this, if
      // there are problems in the future this must be 
      // changed, so the target will know to which
      // validator it belongs.            


      return _effectValidators.Any(
        validator =>
          {
            return validator.HasValidZone(target) &&
              validator.IsTargetValid(target, triggerMessage);
          });
    }
  }
}