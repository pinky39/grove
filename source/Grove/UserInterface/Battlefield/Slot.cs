namespace Grove.UserInterface.Battlefield
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Caliburn.Micro;

  public class Slot : IDisposable
  {
    private readonly Func<UserInterface.Permanent.ViewModel, bool> _accepts;
    private readonly BindableCollection<UserInterface.Permanent.ViewModel> _permanents = new BindableCollection<UserInterface.Permanent.ViewModel>();

    public Slot(Func<UserInterface.Permanent.ViewModel, bool> accepts)
    {
      _accepts = accepts;
    }

    public int Count { get { return _permanents.Count; } }

    public IEnumerable<UserInterface.Permanent.ViewModel> Permanents { get { return _permanents; } }

    public void Add(UserInterface.Permanent.ViewModel viewModel)
    {
      if (viewModel.Card.IsAttached && viewModel.Card.Is().Attachment)
      {
        var insertAt = GetEnchantmentOrEquipmentPosition(viewModel.Card);
        _permanents.Insert(insertAt.Value, viewModel);
        return;
      }

      _permanents.Add(viewModel);
    }

    public void Clear()
    {
      _permanents.Clear();
    }

    public bool CanAdd(UserInterface.Permanent.ViewModel viewModel)
    {
      return viewModel.Card.Is().Aura ? ContainsAttachmentTarget(viewModel.Card) : _accepts(viewModel);
    }

    public UserInterface.Permanent.ViewModel GetPermanentViewModel(Card card)
    {
      return _permanents.FirstOrDefault(x => x.Card == card);
    }

    public bool ContainsAttachmentTarget(Card attachment)
    {
      return _permanents.Any(x => x.Card == attachment.AttachedTo);
    }

    public bool Remove(UserInterface.Permanent.ViewModel viewModel)
    {                  
      return _permanents.Remove(viewModel);      
    }

    private int? GetEnchantmentOrEquipmentPosition(Card card)
    {
      int insertAt;

      for (insertAt = 0; insertAt < _permanents.Count; insertAt++)
      {
        if (_permanents[insertAt].Card.HasAttachment(card))
          break;
      }

      return insertAt < _permanents.Count ? insertAt : (int?) null;
    }

    public void Dispose()
    {
      foreach (var viewModel in _permanents)
      {
        viewModel.Dispose();
      }
    }
  }
}