namespace Grove.Core.Effects
{
  using System.Collections.Generic;

  public class DiscreteEffectChoice
  {
    private readonly EffectOption[] _options;

    public DiscreteEffectChoice(params EffectOption[] options)
    {
      _options = options;
    }

    public IEnumerable<EffectOption> Options { get { return _options; } }
  }
}