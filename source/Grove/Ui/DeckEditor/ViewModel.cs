namespace Grove.Ui.DeckEditor
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Infrastructure;

  public class ViewModel : IIsDialogHost
  {
    private readonly List<object> _largeDialogs = new List<object>();
    private readonly List<Card> _previews;

    public ViewModel(CardDatabase cardDatabase)
    {
      _previews = cardDatabase.CreatePreviewForEveryCard().ToList();

      Library = Bindable.Create<Library>(_previews);
      Deck = Bindable.Create<Deck>(MediaLibrary.GetDeckPath("kuno-b-control"), _previews);
      SelectedCard = Deck.GetCard();
    }

    public Deck Deck { get; private set; }
    public Library Library { get; private set; }
    public object LargeDialog { get { return _largeDialogs.FirstOrDefault(); } }
    public virtual Card SelectedCard { get; protected set; }

    public void AddDialog(object dialog, DialogType dialogType)
    {
      if (dialogType == DialogType.Large)
      {
        _largeDialogs.Add(dialog);
      }
    }

    public void RemoveDialog(object dialog)
    {
      _largeDialogs.Remove(dialog);
    }

    public bool HasFocus(object dialog)
    {
      return dialog == LargeDialog;
    }

    public void CloseAllDialogs()
    {
      foreach (var dialog in _largeDialogs.ToList())
      {
        dialog.Close();
      }
    }

    public void ChangeSelectedCard(string name)
    {
      SelectedCard = _previews.First(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
    }

    public void Open() {}

    public void New() {}

    public void Save() {}

    public void SaveAs() {}

    public void AddCard() {}

    public void RemoveCard() {}

    public interface IFactory
    {
      ViewModel Create();
    }
  }
}