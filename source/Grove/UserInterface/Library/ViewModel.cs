namespace Grove.UserInterface.Library
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Caliburn.Micro;
  using Gameplay;
  using Gameplay.Zones;
  using Infrastructure;

  public class ViewModel : ViewModelBase, IDisposable
  {
    private readonly BindableCollection<SelectableCard.ViewModel> _cards =
      new BindableCollection<SelectableCard.ViewModel>();

    private readonly Player _owner;

    public ViewModel(Player owner)
    {
      _owner = owner;

      owner.Library.CardAdded += OnCardAdded;
      owner.Library.CardRemoved += OnCardRemoved;
      owner.Library.Shuffled += OnLibraryShuffled;
    }

    public IEnumerable<SelectableCard.ViewModel> Cards { get { return _cards; } }

    public override void Initialize()
    {
      _cards.AddRange(_owner.Library.Select(ViewModels.SelectableCard.Create));
    }

    private void OnCardRemoved(object sender, ZoneChangedEventArgs e)
    {
      var viewModel = _cards.Single(x => x.Card == e.Card);

      _cards.Remove(viewModel);
      viewModel.Close();
      ViewModels.SelectableCard.Destroy(viewModel);
    }

    private void OnLibraryShuffled(object sender, EventArgs args)
    {
      _cards.Clear();
      _cards.AddRange(_owner.Library.Select(ViewModels.SelectableCard.Create));
    }

    private void OnCardAdded(object sender, ZoneChangedEventArgs e)
    {
      _cards.Add(ViewModels.SelectableCard.Create(e.Card));
    }    

    public interface IFactory
    {
      ViewModel Create(Player owner);
    }

    public void Dispose()
    {
      foreach (var viewModel in _cards)
      {
        viewModel.Dispose();
      }
    }
  }
}