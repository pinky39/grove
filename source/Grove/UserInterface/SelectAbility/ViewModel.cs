namespace Grove.UserInterface.SelectAbility
{
  using System.Collections.Generic;
  using Infrastructure;

  public class ViewModel
  {    
    private readonly List<CardText> _descriptions = new List<CardText>();
    private int _selectedIndex;

    public ViewModel(IEnumerable<CardText> descriptions, bool canCancel = true)
    {      
      _descriptions.AddRange(descriptions);
      _selectedIndex = -1;
      CanCancel = canCancel;
    }

    public IEnumerable<CardText> Descriptions { get { return _descriptions; } }
    public bool CanCancel { get; private set; }

    public bool WasCanceled { get; private set; }

    public virtual int SelectedIndex
    {
      get { return _selectedIndex; }
      set
      {
        _selectedIndex = value;
        this.Close();
      }
    }

    public void Cancel()
    {      
      WasCanceled = true;
      this.Close();
    }

    public interface IFactory
    {
      ViewModel Create(IEnumerable<CardText> descriptions, bool canCancel = true);
    }
  }
}