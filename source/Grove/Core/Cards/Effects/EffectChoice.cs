namespace Grove.Core.Cards.Effects
{
  using System.Collections.Generic;

  public class EffectChoice
  {
    private readonly List<EffectChoiceOption> _options = new List<EffectChoiceOption>();
    
    public EffectChoice(IEnumerable<EffectChoiceOption> options)
    {
      _options.AddRange(options);
    }
    
    public IEnumerable<EffectChoiceOption> Options {get { return _options; }}
  }
}