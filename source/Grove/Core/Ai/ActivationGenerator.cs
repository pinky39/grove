namespace Grove.Core.Ai
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;

  public class ActivationGenerator : IEnumerable<ActivationParameters>
  {
    private readonly Game _game;
    private readonly SpellPrerequisites _prerequisites;
    private readonly bool _payKicker;
    private readonly Card _spell;

    public ActivationGenerator(Card spell, SpellPrerequisites prerequisites, bool payKicker, Game game)
    {
      _spell = spell;
      _prerequisites = prerequisites;
      _payKicker = payKicker;
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
      var selectors = _payKicker
        ? _prerequisites.KickerTargetSelectors
        : _prerequisites.TargetSelectors;
      
      if (selectors.HasEffect | selectors.HasCost)
      {
        var generator = new TargetGenerator(
          selectors,
          _spell,
          _game,
          _prerequisites.MaxX);

        foreach (var targets in generator)
        {
          yield return new ActivationParameters
            (
            payKicker: _payKicker,
            targets: targets,
            x: CalculateX(targets)
            );
        }
      }
      else
      {
        yield return new ActivationParameters
          (
          payKicker: _payKicker,
          x: CalculateX()
          );
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