namespace Grove.Ui.DeckEditor
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Details.Mana;
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


    public IEnumerable<Card> FilteredResult
    {
      get
      {
        IEnumerable<Card> result = _cards;

        if (!string.IsNullOrEmpty(Name))
        {
          result = result.Where(x => x.Name.StartsWith(Name, StringComparison.InvariantCultureIgnoreCase));
        }

        return result.Where(x =>
          {
            
            
            if (x.ConvertedCost < MinimumCost || x.ConvertedCost > MaximumCost)
              return false;

            if (White && x.HasColors(ManaColors.White))
              return true;

            if (Blue && x.HasColors(ManaColors.Blue))
              return true;

            if (Black && x.HasColors(ManaColors.Black))
              return true;

            if (Red && x.HasColors(ManaColors.Red))
              return true;

            if (Green && x.HasColors(ManaColors.Green))
              return true;

            return false;
          });
      }
    }
  }
}