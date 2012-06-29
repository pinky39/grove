namespace Grove.Core.Ai
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;

  public class ActivationGenerator : IEnumerable<ActivationParameters>
  {
    private readonly Game _game;
    private readonly SpellPrerequisites _prerequisites;
    private readonly Card _spell;

    public ActivationGenerator(Card spell, SpellPrerequisites prerequisites, Game game)
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
      return parameters.X <= _prerequisites.MaxX;
    }

    private IEnumerable<ActivationParameters> GenerateActivations()
    {
      
      if (_prerequisites.NeedsTargets)
      {
        var generator = new TargetGenerator(
          _prerequisites.KickerTargetSelectors,
          _spell,
          _game,
          _prerequisites.MaxX);

        foreach (var targets in generator.Take(Search.TargetLimit))
        {
          yield return new ActivationParameters
            (
            targets: targets,
            x: CalculateX(targets)
            );
        }
      }
      else
      {
        yield return new ActivationParameters
          (
          x: CalculateX()
          );
      }

      if (_prerequisites.CanCastWithKicker)
      {
        if (_prerequisites.NeedsKickerTargets)
        {
          var generator = new TargetGenerator(
            _prerequisites.KickerTargetSelectors,
            _spell,
            _game,
            _prerequisites.MaxX);


          foreach (var targets in generator.Take(Search.TargetLimit))
          {
            yield return new ActivationParameters
              (
              payKicker: true,
              targets: targets,
              x: CalculateX(targets)
              );
          }
        }
        else
        {
          yield return new ActivationParameters(
            payKicker: true,
            x: CalculateX()
            );
        }
      }
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