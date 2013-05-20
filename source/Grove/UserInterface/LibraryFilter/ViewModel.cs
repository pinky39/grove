namespace Grove.UserInterface.LibraryFilter
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Caliburn.Micro;
  using Gameplay;
  using Gameplay.Characteristics;
  using Infrastructure;

  public class ViewModel : ViewModelBase
  {
    private readonly HashSet<string> _cardsNames;
    private readonly Func<Card, object> _transformResult;
    private string _text = String.Empty;

    public ViewModel(IEnumerable<string> cardNames, Func<Card, object> transformResult)
    {
      _transformResult = transformResult ?? (x => x);
      _cardsNames = new HashSet<string>(cardNames);

      White = Blue = Black = Red = Green = true;
      Costs = Enumerable.Range(0, 17).ToArray();
      MinimumCost = Costs.First();
      MaximumCost = Costs.Last();
    }

    public int[] Costs { get; private set; }

    [Updates("FilteredResult")]
    public virtual string Text { get { return _text; } set { _text = value.ToLowerInvariant(); } }

    [Updates("FilteredResult")]
    public virtual bool White { get; set; }

    [Updates("FilteredResult")]
    public virtual bool Blue { get; set; }

    [Updates("FilteredResult")]
    public virtual bool Black { get; set; }

    [Updates("FilteredResult")]
    public virtual bool Red { get; set; }

    [Updates("FilteredResult")]
    public virtual bool Green { get; set; }

    [Updates("FilteredResult")]
    public virtual int MinimumCost { get; set; }

    [Updates("FilteredResult")]
    public virtual int MaximumCost { get; set; }

    public IEnumerable<object> FilteredResult { get { return LoadView(); } }


    private IEnumerable<object> LoadView()
    {
      var view = new BindableCollection<object>();

      var text = _text;

      var task = TaskEx.Delay(300);

      task.ContinueWith(delegate
        {
          if (text != _text)
            return;

          var cards = CardsInfo.Query(_text.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries));
          
          foreach (var card in cards.OrderBy(x => x.Name))
          {
            if (!_cardsNames.Contains(card.Name))
              continue;
            
            if (card.ConvertedCost < MinimumCost || card.ConvertedCost > MaximumCost)
              continue;

            if (
              (White && card.HasColor(CardColor.White)) ||
                (Blue && card.HasColor(CardColor.Blue)) ||
                  (Black && card.HasColor(CardColor.Black)) ||
                    (Red && card.HasColor(CardColor.Red)) ||
                      (Green && card.HasColor(CardColor.Green)) ||
                        (card.HasColor(CardColor.Colorless) || card.ManaCost == null)
              )
            {
              view.Add(_transformResult(card));
            }
          }
        });


      return view;
    }

    public interface IFactory
    {
      ViewModel Create(IEnumerable<string> cardNames, Func<Card, object> transformResult);
    }
  }
}