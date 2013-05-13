namespace Grove.UserInterface.LibraryFilter
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using Caliburn.Micro;
  using Gameplay;
  using Gameplay.Characteristics;
  using Infrastructure;

  public class ViewModel : ViewModelBase
  {
    private readonly List<Card> _cards = new List<Card>();
    private readonly List<string> _cardsNames = new List<string>();
    private readonly Func<Card, object> _transformResult;
    
    public ViewModel(IEnumerable<string> cardNames, Func<Card, object> transformResult)
    {
      _transformResult = transformResult ?? (x => x);
      _cardsNames.AddRange(cardNames.OrderBy(x => x));

      White = Blue = Black = Red = Green = true;
      Costs = Enumerable.Range(0, 17).ToArray();
      MinimumCost = Costs.First();
      MaximumCost = Costs.Last();
    }

    public int[] Costs { get; private set; }

    [Updates("FilteredResult")]
    public virtual string Name { get; set; }

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

    public override void Initialize()
    {
      _cards.AddRange(_cardsNames.Select(x => CardsInfo[x]));
    }

    private IEnumerable<object> LoadView()
    {
      var view = new BindableCollection<object>();

      Task.Factory.StartNew(() =>
        {
          foreach (var card in _cards)
          {
            if (!string.IsNullOrEmpty(Name))
            {
              if (!card.Name.StartsWith(Name, StringComparison.InvariantCultureIgnoreCase))
              {
                continue;
              }
            }

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