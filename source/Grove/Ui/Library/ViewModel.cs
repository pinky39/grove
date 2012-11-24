namespace Grove.Ui.Library
{
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.Linq;
  using Caliburn.Micro;
  using Core;
  using Core.Zones;
  using Infrastructure;

  public class ViewModel
  {
    private readonly SelectableCard.ViewModel.IFactory _cardVmFactory;

    private readonly BindableCollection<SelectableCard.ViewModel> _cards =
      new BindableCollection<SelectableCard.ViewModel>();

    private readonly ILibraryQuery _library;

    public ViewModel(ILibraryQuery library, SelectableCard.ViewModel.IFactory cardVmFactory)
    {
      CardVmFactory = cardVmFactory;
      _library = library;
      _cardVmFactory = cardVmFactory;
      _cards.AddRange(library.Select(cardVmFactory.Create));

      ((INotifyCollectionChanged) library).CollectionChanged += Synchronize;
    }

    public SelectableCard.ViewModel.IFactory CardVmFactory { get; set; }

    public IEnumerable<SelectableCard.ViewModel> Cards { get { return _cards; } }

    private void Synchronize(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.Action == NotifyCollectionChangedAction.Reset)
      {
        _cards.Clear();
        _cards.AddRange(_library.Select(_cardVmFactory.Create));
      }

      if (e.Action == NotifyCollectionChangedAction.Add)
      {
        foreach (Card card in e.NewItems)
        {
          _cards.Add(_cardVmFactory.Create(card));
        }
      }

      if (e.Action == NotifyCollectionChangedAction.Remove)
      {
        foreach (Card card in e.OldItems)
        {
          var viewModel = _cards.Single(x => x.Card == card);

          _cards.Remove(viewModel);
          viewModel.Close();
        }
      }
    }

    public interface IFactory
    {
      ViewModel Create(ILibraryQuery library);
    }
  }
}