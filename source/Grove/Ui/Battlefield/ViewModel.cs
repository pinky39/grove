namespace Grove.Ui.Battlefield
{
  using System.Collections.Specialized;
  using System.Linq;
  using Core;
  using Core.Messages;
  using Core.Zones;
  using Infrastructure;

  public class ViewModel : IReceive<AttachmentAttached>, IReceive<AttachmentDetached>
  {
    private readonly Player _owner;

    private readonly Row[] _rows = new[]
      {
        new Row(
          LandSlot(),
          LandSlot(),
          LandSlot(),
          LandSlot(),
          LandSlot(),
          LandSlot(),
          LandSlot(),
          LandSlot()),
        new Row(
          CreatureSlot(),
          CreatureSlot(),
          CreatureSlot(),
          CreatureSlot(),
          CreatureSlot(),
          MiscSlot(),
          MiscSlot(),
          MiscSlot()
          ),
      };

    private readonly Permanent.ViewModel.IFactory _viewModelFactory;

    public ViewModel(Permanent.ViewModel.IFactory viewModelFactory, Player owner, Game game)
    {
      _viewModelFactory = viewModelFactory;
      _owner = owner;

      SwitchRows = owner == game.Players.Player2;

      ((INotifyCollectionChanged) _owner.Battlefield).CollectionChanged += Synchronize;
    }

    public Row Row1 { get { return SwitchRows ? _rows[1] : _rows[0]; } }
    public Row Row2 { get { return SwitchRows ? _rows[0] : _rows[1]; } }
    public bool SwitchRows { get; private set; }

    public void Receive(AttachmentAttached message)
    {
      if (message.Attachment.Controller == _owner && message.Attachment.Is().Equipment)
      {
        Attach(message.Attachment);
      }
    }

    public void Receive(AttachmentDetached message)
    {
      if (message.Attachment.Controller == _owner && message.Attachment.Zone == Zone.Battlefield)
      {
        Detach(message.Attachment);
      }
    }


    private static Slot CreatureSlot()
    {
      return new Slot(vm => vm.Card.Is().Creature);
    }

    private static Slot LandSlot()
    {
      return new Slot(vm => vm.Card.Is().Land);
    }

    private static Slot MiscSlot()
    {
      return new Slot(vm => !vm.Card.Is().Creature && !vm.Card.Is().Land);
    }

    private void Add(Permanent.ViewModel viewModel)
    {
      var row = _rows.First(r => r.CanAdd(viewModel));
      row.Add(viewModel);
    }

    private void Attach(Card attachment)
    {
      var viewModel = Remove(attachment);

      foreach (var row in _rows)
      {
        if (row.ContainsAttachmentTarget(attachment))
        {
          row.Add(viewModel);
          break;
        }
      }
    }

    private void Detach(Card equipment)
    {
      var viewModel = Remove(equipment);

      if (viewModel != null)
        Add(viewModel);
    }

    private Permanent.ViewModel Remove(Card card)
    {
      foreach (var row in _rows)
      {
        var viewModel = row.GetPermanent(card);

        if (viewModel != null)
        {
          row.Remove(viewModel);
          return viewModel;
        }
      }

      return null;
    }

    private void Synchronize(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.Action == NotifyCollectionChangedAction.Add)
      {
        foreach (Card card in e.NewItems)
        {
          var viewModel = _viewModelFactory.Create(card);
          Add(viewModel);
        }
      }

      if (e.Action == NotifyCollectionChangedAction.Remove)
      {
        foreach (Card card in e.OldItems)
        {
          var viewModel = Remove(card);
          viewModel.Close();
        }
      }
    }

    public interface IFactory
    {
      ViewModel Create(Player owner);
    }
  }
}