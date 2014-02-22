namespace Grove.UserInterface.Battlefield
{
  using System;
  using System.Linq;
  using Gameplay;
  using Gameplay.Messages;
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
          LandSlot()),
        new Row(
          CreatureSlot(),
          CreatureSlot(),
          CreatureSlot(),
          CreatureSlot(),
          CreatureSlot(),
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

    public void Dispose()
    {
      foreach (var row in _rows)
      {
        row.Dispose();
      }
    }

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
      foreach (var card in _owner.Battlefield.Where(x => !x.IsAttached))
      {
        AddCard(card);
      }

      foreach (var card in _owner.Battlefield.Where(x => x.IsAttached))
      {
        AddCard(card);
      }

      _owner.Battlefield.CardAdded += OnCardAdded;
      _owner.Battlefield.CardRemoved += OnCardRemoved;
    }

    private void OnCardRemoved(object sender, ZoneChangedEventArgs e)
    {
      var viewModel = GetPermanent(e.Card);
      viewModel.OnPermanentLeftBattlefield();
      ViewModels.Permanent.Destroy(viewModel);

      Remove(viewModel);
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

    private void Add(UserInterface.Permanent.ViewModel viewModel)
    {
      var row = _rows.First(r => r.CanAdd(viewModel));
      row.Add(viewModel);
    }

    private void Attach(Card attachment)
    {
      var viewModel = GetPermanent(attachment);
      Remove(viewModel);

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
      var viewModel = GetPermanent(equipment);

      if (viewModel != null)
      {
        Remove(viewModel);
        Add(viewModel);
      }
    }

    private UserInterface.Permanent.ViewModel GetPermanent(Card card)
    {
      foreach (var row in _rows)
      {
        var viewModel = row.GetPermanent(card);

        if (viewModel != null)
        {
          return viewModel;
        }
      }

      return null;
    }

    private void Remove(UserInterface.Permanent.ViewModel permanent)
    {
      foreach (var row in _rows)
      {
        var removed = row.Remove(permanent);

        if (removed)
          break;
      }
    }

    public interface IFactory
    {
      ViewModel Create(Player owner);
    }
  }
}