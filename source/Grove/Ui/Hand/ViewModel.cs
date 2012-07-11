namespace Grove.Ui.Hand
{
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.Linq;
  using Caliburn.Micro;
  using Core;
  using Infrastructure;

  public class ViewModel
  {
    private readonly BindableCollection<Spell.ViewModel> _cards = new BindableCollection<Spell.ViewModel>();
    private readonly Spell.ViewModel.IFactory _spellVmFactory;

    public ViewModel(Spell.ViewModel.IFactory spellVmFactory, IEnumerable<Card> hand)
    {
      _cards.AddRange(hand.Select(spellVmFactory.Create));

      ((INotifyCollectionChanged) hand).CollectionChanged += Synchronize;
      _spellVmFactory = spellVmFactory;
    }

    public IEnumerable<Spell.ViewModel> Cards { get { return _cards; } }

    private void Synchronize(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.Action == NotifyCollectionChangedAction.Reset)
      {
        _cards.Clear();
      }

      if (e.Action == NotifyCollectionChangedAction.Add)
      {
        foreach (Card card in e.NewItems)
        {
          _cards.Add(_spellVmFactory.Create(card));
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
      ViewModel Create(IEnumerable<Card> hand);
    }
  }
}