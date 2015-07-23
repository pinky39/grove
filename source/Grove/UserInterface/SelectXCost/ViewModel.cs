namespace Grove.UserInterface.SelectXCost
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  public class ViewModel
  {    
    private readonly List<int> _validXChoices = new List<int>();

    private int _chosenX;

    public ViewModel(int maxX, bool canCancel)
    {
      CanCancel = canCancel;
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

    public bool CanCancel { get; private set; }

    public IEnumerable<int> ValidXChoices { get { return _validXChoices; } }

    public bool WasCanceled { get; private set; }

    public void Cancel()
    {
      if (!CanCancel)
        return;

      WasCanceled = true;
      this.Close();
    }

    public interface IFactory
    {
      ViewModel Create(int maxX, bool canCancel = true);
    }
  }
}