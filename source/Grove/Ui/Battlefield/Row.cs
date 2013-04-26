namespace Grove.Ui.Battlefield
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay.Card;

  public class Row
  {
    private readonly List<Slot> _slots = new List<Slot>();

    public Row(params Slot[] slots)
    {
      _slots.AddRange(slots);
    }

    public IEnumerable<Slot> Slots { get { return _slots; } }

    public void Add(Permanent.ViewModel viewModel)
    {
      Slot candidate;

      if (viewModel.Card.IsAttached)
      {
        candidate = _slots.First(slot => slot.ContainsAttachmentTarget(viewModel.Card));
      }
      else
      {
        candidate = _slots
          .Where(slot => slot.CanAdd(viewModel))
          .OrderBy(slot => slot.Count)
          .First();
      }

      candidate.Add(viewModel);
    }

    public bool CanAdd(Permanent.ViewModel viewModel)
    {
      return _slots.Any(slot => slot.CanAdd(viewModel));
    }

    public Permanent.ViewModel GetPermanent(Card card)
    {
      foreach (var slot in Slots)
      {
        var viewModel = slot.GetPermanentViewModel(card);

        if (viewModel != null)
          return viewModel;
      }

      return null;
    }

    public void Remove(Permanent.ViewModel viewModel)
    {
      foreach (var slot in Slots)
      {
        var removed = slot.Remove(viewModel);
        if (removed)
          break;
      }
    }

    public void Clear()
    {
      foreach (var slot in Slots)
      {
        slot.Clear();
      }
    }

    public bool ContainsAttachmentTarget(Card attachment)
    {
      return _slots.Any(slot => slot.ContainsAttachmentTarget(attachment));
    }
  }
}