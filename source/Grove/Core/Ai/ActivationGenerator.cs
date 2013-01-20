namespace Grove.Core.Ai
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;

  public class ActivationGenerator : IEnumerable<ActivationParameters>
  {
    private readonly Game _game;

    private readonly ActivationPrerequisites _prerequisites;
    private readonly Card _spell;

    public ActivationGenerator(ActivationPrerequisites prerequisites, Game game)
    {
      _spell = spell;
      _prerequisites = prerequisites;
      _game = game;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public IEnumerator<ActivationParameters> GetEnumerator()
    {
      return GenerateActivations().Where(AreValid).GetEnumerator();
    }

    private bool AreValid(ActivationParameters parameters)
    {
      return !(parameters.X > _prerequisites.MaxX);
    }

    private IEnumerable<ActivationParameters> GenerateActivations()
    {
      
      if (_prerequisites.RequiresTargets)
      {
        var generator = new TargetGenerator(
          _prerequisites.TargetSelector,
          _spell,
          _game,
          _prerequisites.MaxX);

        foreach (var targets in generator)
        {
          yield return new ActivationParameters {Targets = targets, X = CalculateX(targets)};
        }

        yield break;
      }

      yield return new ActivationParameters {X = CalculateX()};
    }

    private int? CalculateX(Targets targets = null)
    {
      return
        _prerequisites.XCalculator != null
          ? _prerequisites.XCalculator(new XCalculatorParameters(_spell, targets, _game))
          : (int?) null;
    }
  }
}