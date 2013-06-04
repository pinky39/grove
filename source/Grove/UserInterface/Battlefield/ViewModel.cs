namespace Grove.UserInterface.Battlefield
{
  using System;
  using System.Linq;
  using Gameplay;
  using Gameplay.Messages;
  using Gameplay.Zones;
  using Infrastructure;

  public class ViewModel : ViewModelBase, IReceive<AttachmentAttached>, IReceive<AttachmentDetached>, IDisposable
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


    public ViewModel(Player owner)
    {
      _owner = owner;
    }

    public Row Row1 { get { return SwitchRows ? _rows[1] : _rows[0]; } }
    public Row Row2 { get { return SwitchRows ? _rows[0] : _rows[1]; } }
    public bool SwitchRows { get { return _owner == Players.Player2; } }

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

    public override void Initialize()
    {
      foreach (var card in _owner.Battlefield)
      {
        AddCard(card);
      }

      foreach (var card in _owner.Battlefield)
      {
        if (card.AttachedTo != null)
        {
          Attach(card);
        }
      }

      _owner.Battlefield.CardAdded += OnCardAdded;
      _owner.Battlefield.CardRemoved += OnCardRemoved;
    }

    private void OnCardRemoved(object sender, ZoneChangedEventArgs e)
    {
      var viewModel = Remove(e.Card);
      viewModel.Close();
      ViewModels.Permanent.Destroy(viewModel);
    }

    private void OnCardAdded(object sender, ZoneChangedEventArgs e)
    {
      AddCard(e.Card);
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

    private void AddCard(Card card)
    {
      var viewModel = ViewModels.Permanent.Create(card);
      Add(viewModel);
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

    public interface IFactory
    {
      ViewModel Create(Player owner);
    }

    public void Dispose()
    {
      foreach (var row in _rows)
      {
        row.Dispose();
      }
    }
  }
}