namespace Grove.Ui.SelectXCost
{
  using System.Collections.Generic;
  using System.Linq;
  using Castle.Core;
  using Infrastructure;

  [Transient]
  public class ViewModel
  {
    private readonly List<int> _validXChoices = new List<int>();

    private int _chosenX;

    public ViewModel(int maxX)
    {
      _validXChoices.AddRange(Enumerable.Range(0, maxX + 1));
    }

    public int ChosenX
    {
      get { return _chosenX; }
      set
      {
        _chosenX = value;
        this.Close();
      }
    }

    public IEnumerable<int> ValidXChoices { get { return _validXChoices; } }

    public bool WasCanceled { get; private set; }

    public void Cancel()
    {
      WasCanceled = true;
      this.Close();
    }

    public interface IFactory
    {
      ViewModel Create(int maxX);
    }
  }
}