namespace Grove.Ui.DeckEditor
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using Caliburn.Micro;
  using Core;
  using Gameplay.Card;
  using Gameplay.Card.Characteristics;
  using Infrastructure;

  public class Library
  {
    private readonly List<Card> _cards = new List<Card>();    

    public Library(IEnumerable<Card> cards)
    {
      _cards.AddRange(cards.OrderBy(x => x.Name));

      White = Blue = Black = Red = Green = true;

      Costs = Enumerable.Range(0, 17).ToArray();
      MinimumCost = Costs.First();
      MaximumCost = Costs.Last();
    }

    public int[] Costs { get; set; }

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

    public IEnumerable<Card> FilteredResult { get { return LoadView(); } }

    private IEnumerable<Card> LoadView()
    {      
      var view = new BindableCollection<Card>();      
      
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
              view.Add(card);
            }
          }
        });

      return view;
    }
  }
}