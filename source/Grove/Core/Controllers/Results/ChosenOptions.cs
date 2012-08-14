namespace Grove.Core.Controllers.Results
{
  using System.Collections.Generic;
  using Details.Cards.Effects;

  public class ChosenOptions
  {
    private readonly List<EffectChoiceOption> _options = new List<EffectChoiceOption>();
        
    public ChosenOptions(IEnumerable<EffectChoiceOption> options)
    {
      _options.AddRange(options);
    }

    public ChosenOptions(params EffectChoiceOption[] options)
    {
      _options.AddRange(options);
    }

    public IList<EffectChoiceOption> Options {get { return _options; }}
  }
}