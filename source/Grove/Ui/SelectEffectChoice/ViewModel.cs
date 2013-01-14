namespace Grove.Ui.SelectEffectChoice
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Core.Decisions.Results;
  using Core.Effects;
  using Infrastructure;

  public class ViewModel
  {
    private readonly List<EffectChoiceViewModel> _choices;

    public ViewModel(IEnumerable<EffectChoice> choices, string text)
    {
      _choices = choices.Select(x => new EffectChoiceViewModel(x)).ToList();

      Message = text
        .Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries)
        .Select(token =>
          {
            if (token.StartsWith("#"))
            {
              var index = int.Parse(token.Substring(1, 1));
              return _choices[index];
            }

            return (object) token;
          })
        .ToList();
    }

    public List<object> Message { get; private set; }
    public ChosenOptions ChosenOptions { get { return new ChosenOptions(_choices.Select(x => x.Selected).ToList()); } }

    public void Done()
    {
      this.Close();
    }

    public interface IFactory
    {
      ViewModel Create(IEnumerable<EffectChoice> choices, string text);
    }
  }
}