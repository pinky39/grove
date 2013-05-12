namespace Grove.UserInterface.Graveyard
{
  using System.Collections.Generic;
  using System.Linq;
  using Caliburn.Micro;
  using Gameplay;
  using Gameplay.Zones;
  using Infrastructure;

  public class ViewModel : ViewModelBase
  {
    private readonly BindableCollection<SelectableCard.ViewModel> _cards =
      new BindableCollection<SelectableCard.ViewModel>();

    public ViewModel(Player owner)
    {
      owner.Graveyard.CardAdded += OnCardAdded;
      owner.Graveyard.CardRemoved += OnCardRemoved;
    }

    public IEnumerable<SelectableCard.ViewModel> Cards { get { return _cards; } }

    private void OnCardRemoved(object sender, ZoneChangedEventArgs e)
    {
      var viewModel = _cards.Single(x => x.Card == e.Card);

      _cards.Remove(viewModel);
      viewModel.Close();
      ViewModels.SelectableCard.Destroy(viewModel);
    }

    private void OnCardAdded(object sender, ZoneChangedEventArgs e)
    {
      _cards.Add(ViewModels.SelectableCard.Create(e.Card));
    }

    public interface IFactory
    {
      ViewModel Create(Player owner);
    }
  }
}