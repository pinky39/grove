namespace Grove.Ui.DeckEditor
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Infrastructure;
  using Shell;

  public class ViewModel : IIsDialogHost
  {
    private static readonly Random Rnd = new Random();
    private readonly List<object> _largeDialogs = new List<object>();
    private readonly List<Card> _previews;
    private readonly IIsDialogHost _previousScreen;
    private readonly IShell _shell;

    public ViewModel(CardDatabase cardDatabase, IIsDialogHost previousScreen, IShell shell)
    {
      _previousScreen = previousScreen;
      _shell = shell;
      _previews = cardDatabase.CreatePreviewForEveryCard().ToList();

      Library = Bindable.Create<Library>(_previews);
      Deck = Deck.GetRandomDeck(_previews);

      SelectedCard = Deck.GetPreviewCard() ?? GetRandomPreview();
    }

    public virtual Deck Deck { get; protected set; }
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

    private Card GetRandomPreview()
    {
      return _previews[Rnd.Next(0, _previews.Count)];
    }

    public void ChangeSelectedCard(string name)
    {
      SelectedCard = GetCard(name);
    }

    private Card GetCard(string name)
    {
      return _previews.First(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
    }

    public void Open() {}
        
    public void NewDeck()
    {
      Deck = new Deck();
    }

    public void Save() {}

    public void SaveAs() {}

    public void Back()
    {
      _shell.ChangeScreen(_previousScreen);
    }

    [Updates("Deck")]
    public virtual void AddToDeck(string name)
    {
      Deck.AddCard(GetCard(name));
    }

    [Updates("Deck")]
    public virtual void RemoveFromDeck(string name)
    {
      Deck.RemoveCard(name);
    }

    public interface IFactory
    {
      ViewModel Create(IIsDialogHost previousScreen);
    }
  }
}