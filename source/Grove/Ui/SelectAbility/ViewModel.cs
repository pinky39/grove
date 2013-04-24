namespace Grove.Ui.SelectAbility
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Characteristics;
  using Infrastructure;

  public class ViewModel
  {
    private readonly List<CardText> _descriptions = new List<CardText>();
    private int _selectedIndex;

    public ViewModel(IEnumerable<CardText> descriptions)
    {
      _descriptions.AddRange(descriptions);
      _selectedIndex = -1;
    }

    public IEnumerable<CardText> Descriptions { get { return _descriptions; } }

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
      ViewModel Create(IEnumerable<CardText> descriptions);
    }
  }
}