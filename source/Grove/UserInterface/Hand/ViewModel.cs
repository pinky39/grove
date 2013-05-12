namespace Grove.UserInterface.Hand
{
  using System.Collections.Generic;
  using System.Linq;
  using Caliburn.Micro;
  using Gameplay;
  using Gameplay.Zones;
  using Infrastructure;

  public class ViewModel : ViewModelBase
  {
    private readonly BindableCollection<Spell.ViewModel> _cards = new BindableCollection<Spell.ViewModel>();

    public ViewModel(Player owner)
    {
      owner.Hand.CardAdded += OnCardAdded;
      owner.Hand.CardRemoved += OnCardRemoved;
    }

    public IEnumerable<Spell.ViewModel> Cards { get { return _cards; } }

    private void OnCardRemoved(object sender, ZoneChangedEventArgs e)
    {
      var viewModel = _cards.Single(x => x.Card == e.Card);

      _cards.Remove(viewModel);

      viewModel.Close();
      ViewModels.Spell.Destroy(viewModel);
    }

    private void OnCardAdded(object sender, ZoneChangedEventArgs e)
    {
      _cards.Add(ViewModels.Spell.Create(e.Card));
    }

    public interface IFactory
    {
      ViewModel Create(Player owner);
    }
  }
}