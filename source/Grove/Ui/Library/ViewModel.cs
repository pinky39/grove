namespace Grove.Ui.Library
{
  using System;
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

    private readonly Player _owner;

    public ViewModel(Player owner, SelectableCard.ViewModel.IFactory cardVmFactory)
    {
      CardVmFactory = cardVmFactory;
      _owner = owner;
      _cardVmFactory = cardVmFactory;

      _cards.AddRange(owner.Library.Select(cardVmFactory.Create));

      owner.Library.CardAdded += OnCardAdded;
      owner.Library.CardRemoved += OnCardRemoved;
      owner.Library.Shuffled += OnLibraryShuffled;
    }

    public SelectableCard.ViewModel.IFactory CardVmFactory { get; set; }

    public IEnumerable<SelectableCard.ViewModel> Cards { get { return _cards; } }

    private void OnCardRemoved(object sender, ZoneChangedEventArgs e)
    {
      var viewModel = _cards.Single(x => x.Card == e.Card);

      _cards.Remove(viewModel);
      viewModel.Close();
      _cardVmFactory.Destroy(viewModel);
    }

    private void OnLibraryShuffled(object sender, EventArgs args)
    {
      _cards.Clear();
      _cards.AddRange(_owner.Library.Select(_cardVmFactory.Create));
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