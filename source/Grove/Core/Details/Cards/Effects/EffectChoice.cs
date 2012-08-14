namespace Grove.Core.Details.Cards.Effects
{
  using System.Collections.Generic;

  public class EffectChoice
  {
    private List<EffectChoiceOption> _options = new List<EffectChoiceOption>();
    
    public EffectChoice(IEnumerable<EffectChoiceOption> options)
    {
      _options.AddRange(options);
    }
    
    public IEnumerable<EffectChoiceOption> Options {get { return _options; }}
  }
}