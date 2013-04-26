namespace Grove.Ui.Graveyard
{
  using System.Collections.Generic;
  using System.Linq;
  using Caliburn.Micro;
  using Gameplay.Player;
  using Gameplay.Zones;
  using Infrastructure;

  public class ViewModel
  {
    private readonly SelectableCard.ViewModel.IFactory _cardVmFactory;

    private readonly BindableCollection<SelectableCard.ViewModel> _cards =
      new BindableCollection<SelectableCard.ViewModel>();

    public ViewModel(Player owner, SelectableCard.ViewModel.IFactory cardVmFactory)
    {
      owner.Graveyard.CardAdded += OnCardAdded;
      owner.Graveyard.CardRemoved += OnCardRemoved;
      _cardVmFactory = cardVmFactory;
    }

    public IEnumerable<SelectableCard.ViewModel> Cards { get { return _cards; } }

    private void OnCardRemoved(object sender, ZoneChangedEventArgs e)
    {
      var viewModel = _cards.Single(x => x.Card == e.Card);

      _cards.Remove(viewModel);
      viewModel.Close();
      _cardVmFactory.Destroy(viewModel);
    }

    private void OnCardAdded(object sender, ZoneChangedEventArgs e)
    {
      _cards.Add(_cardVmFactory.Create(e.Card));
    }


    public interface IFactory
    {
      ViewModel Create(Player owner);
    }
  }
}