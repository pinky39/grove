using Caliburn.Micro;
using Grove.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grove.UserInterface.Exile
{
  public class ViewModel : ViewModelBase, IDisposable
  {
    private readonly BindableCollection<SelectableCard.ViewModel> _cards =
      new BindableCollection<SelectableCard.ViewModel>();

    private readonly Player _owner;

    public ViewModel(Player owner)
    {
      _owner = owner;
    }

    public IEnumerable<SelectableCard.ViewModel> Cards { get { return _cards; } }

    public override void Initialize()
    {
      foreach (var card in _owner.Exile)
      {
        AddCard(card);
      }

      _owner.Exile.CardAdded += OnCardAdded;
      _owner.Exile.CardRemoved += OnCardRemoved;
    }

    private void OnCardRemoved(object sender, ZoneChangedEventArgs e)
    {
      var viewModel = _cards.Single(x => x.Card == e.Card);

      _cards.Remove(viewModel);
      viewModel.Close();
      ViewModels.SelectableCard.Destroy(viewModel);
    }

    private void OnCardAdded(object sender, ZoneChangedEventArgs e)
    {
      AddCard(e.Card);
    }

    private void AddCard(Card card)
    {
      _cards.Add(ViewModels.SelectableCard.Create(card));
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
