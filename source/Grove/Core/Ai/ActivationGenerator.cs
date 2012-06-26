namespace Grove.Core.Ai
{
  using System;
  using System.Collections;
  using System.Collections.Generic;

  public class ActivationGenerator : IEnumerable<ActivationParameters>
  {
    private readonly List<ITarget> _costTargets = new List<ITarget>();
    private readonly List<ITarget> _effectTargets = new List<ITarget>();
    private readonly List<ITarget> _kickerEffectTargets = new List<ITarget>();
    private readonly Card _spell;
    private readonly SpellPrerequisites _prerequisites;
    private readonly Players _players;

    public ActivationGenerator(Card spell, SpellPrerequisites prerequisites, Players players, Zones.Stack stack)
    {
      _spell = spell;
      _prerequisites = prerequisites;
      _players = players;

      if (prerequisites.NeedsCostTargets)
      {
        _costTargets.AddRange(new TargetGenerator(
          prerequisites.CostTargetSelector,
          players,
          stack,
          prerequisites.MaxX));
      }

      if (prerequisites.NeedsEffectTargets)
      {
        _effectTargets.AddRange(new TargetGenerator(
          prerequisites.EffectTargetSelector,
          players,
          stack,
          prerequisites.MaxX));
      }

      if (prerequisites.NeedsKickerEffectTargets)
      {
        _kickerEffectTargets.AddRange(new TargetGenerator(
          prerequisites.KickerTargetSelector,
          players,
          stack,
          prerequisites.MaxX)
          );
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public IEnumerator<ActivationParameters> GetEnumerator()
    {
      return GenerateActivations().GetEnumerator();
    }

    private IEnumerable<Func<ActivationParameters>> ActivationsWithCostTargets()
    {
      foreach (var costTarget in _costTargets)
      {
        var target = costTarget;

        yield return () => new ActivationParameters{
          CostTarget = target
        };

        if (_prerequisites.CanCastWithKicker)
          yield return () => new ActivationParameters{
            PayKicker = true,
            CostTarget = target
          };
      }
    }

    private IEnumerable<Func<ActivationParameters>> ActivationsWithoutCostTargets()
    {
      yield return () => new ActivationParameters();

      if (_prerequisites.CanCastWithKicker)
        yield return () => new ActivationParameters{
          PayKicker = true
        };
    }


    private IEnumerable<ActivationParameters> GenerateActivations()
    {
      foreach (var factory in GetAllPossibleActivationCosts())
      {
        var activation = factory();

        var targets = activation.PayKicker
          ? _kickerEffectTargets
          : _effectTargets;

        var needsTargets = activation.PayKicker
          ? _prerequisites.NeedsKickerEffectTargets
          : _prerequisites.NeedsEffectTargets;

        if (needsTargets && targets.Count == 0)
          continue;

        if (targets.Count == 0)
        {
          if (_prerequisites.HasXInCost)
          {
            activation.X = GetX();

            if (activation.X > _prerequisites.MaxX)
              continue;
          }

          yield return activation;
          continue;
        }
        
        // since only spells with one target are 
        // allowed each target represents another combination
        foreach (var effectTarget in targets)
        {
          if (_prerequisites.HasXInCost)
          {
            activation.X = GetX(effectTarget);

            if (activation.X > _prerequisites.MaxX)
              continue;
          }

          activation.EffectTarget = effectTarget;
          yield return activation;

          activation = factory();
        }
      }
    }

    private IEnumerable<Func<ActivationParameters>> GetAllPossibleActivationCosts()
    {
      return _costTargets.Count > 0
        ? ActivationsWithCostTargets()
        : ActivationsWithoutCostTargets();
    }

    private int GetX(ITarget target = null)
    {
      return _prerequisites.XCalculator(_players, _spell, target);
    }
  }
}