namespace Grove.Ui.Hand
{
  using System.Collections.Generic;
  using System.Linq;
  using Caliburn.Micro;
  using Core;
  using Core.Zones;
  using Infrastructure;

  public class ViewModel
  {
    private readonly BindableCollection<Spell.ViewModel> _cards = new BindableCollection<Spell.ViewModel>();
    private readonly Spell.ViewModel.IFactory _spellVmFactory;

    public ViewModel(Spell.ViewModel.IFactory spellVmFactory, Player owner)
    {
      _spellVmFactory = spellVmFactory;

      owner.Hand.CardAdded += OnCardAdded;
      owner.Hand.CardRemoved += OnCardRemoved;
    }

    public IEnumerable<Spell.ViewModel> Cards { get { return _cards; } }

    private void OnCardRemoved(object sender, ZoneChangedEventArgs e)
    {
      var viewModel = _cards.Single(x => x.Card == e.Card);

      _cards.Remove(viewModel);
      
      viewModel.Close();
      _spellVmFactory.Destroy(viewModel);
    }

    private void OnCardAdded(object sender, ZoneChangedEventArgs e)
    {
      _cards.Add(_spellVmFactory.Create(e.Card));
    }

    public interface IFactory
    {
      ViewModel Create(Player owner);      
    }
  }
}