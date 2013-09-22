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
    private readonly Func<CardInfo, int> _orderBy;
    private readonly Dictionary<string, CardInfo> _cards;
    private readonly Func<CardInfo, object> _transformResult;

    public ViewModel(IEnumerable<CardInfo> cards, Func<CardInfo, object> transformResult, Func<CardInfo, int> orderBy)
    {
      _orderBy = orderBy;
      _transformResult = transformResult ?? (x => x);
      _cards = cards.ToDictionary(x => x.Name, x => x);

      White = Blue = Black = Red = Green = true;      
    }    

    [Updates("FilteredResult")]
    public virtual string Text { get; set;  }

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

    public IEnumerable<object> FilteredResult { get { return LoadView(); } }


    private IEnumerable<object> LoadView()
    {
      var view = new BindableCollection<object>();

      var text = Text;

      var task = TaskEx.Delay(1000);

      task.ContinueWith(delegate
        {
          if (text != Text)
            return;

          var cards = CardDatabase
            .Query(text)
            .Where(x => _cards.ContainsKey(x.Name));

          foreach (var card in cards.OrderBy(x => _orderBy(_cards[x.Name])).ThenBy(x => x.Name))
          {            
            var info = _cards[card.Name];         

            if (
              (White && card.HasColor(CardColor.White)) ||
                (Blue && card.HasColor(CardColor.Blue)) ||
                  (Black && card.HasColor(CardColor.Black)) ||
                    (Red && card.HasColor(CardColor.Red)) ||
                      (Green && card.HasColor(CardColor.Green)) ||
                        (card.HasColor(CardColor.Colorless) || card.ManaCost == null)
              )
            {
              view.Add(_transformResult(info));
            }
          }
        });


      return view;
    }

    public interface IFactory
    {
      ViewModel Create(IEnumerable<CardInfo> cards, Func<CardInfo, object> transformResult, Func<CardInfo, int> orderBy);
    }
  }
}