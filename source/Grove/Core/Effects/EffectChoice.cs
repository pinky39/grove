namespace Grove.Core.Effects
{
  using System.Collections.Generic;

  public class EffectChoice
  {
    private readonly EffectChoiceOption[] _options;

    public EffectChoice(params EffectChoiceOption[] options)
    {
      _options = options;
    }

    public IEnumerable<EffectChoiceOption> Options {get { return _options; }}
  }
}