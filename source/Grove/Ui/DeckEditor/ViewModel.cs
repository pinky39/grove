namespace Grove.Ui.DeckEditor
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Windows;
  using Core;
  using Infrastructure;
  using SelectDeck;
  using Shell;

  public class ViewModel : IIsDialogHost
  {
    private const string NewDeckName = "new deck";
    private readonly List<object> _dialogs = new List<object>();
    private readonly CardPreviews _previews;
    private readonly IIsDialogHost _previousScreen;
    private readonly SaveDeckAs.ViewModel.IFactory _saveDeckAsFactory;
    private readonly SelectDeck.ViewModel.IFactory _selectDeckScreenFactory;
    private readonly IShell _shell;
    private bool _isNewDeck;
    private bool _isSaved;

    public ViewModel(IIsDialogHost previousScreen, IShell shell,
      SelectDeck.ViewModel.IFactory selectDeckScreenFactory, SaveDeckAs.ViewModel.IFactory saveDeckAsFactory,
      CardPreviews previews)
    {
      _previousScreen = previousScreen;
      _shell = shell;
      _selectDeckScreenFactory = selectDeckScreenFactory;
      _saveDeckAsFactory = saveDeckAsFactory;
      _previews = previews;
      _isSaved = true;

      Library = Bindable.Create<Library>(_previews);
      Deck = Deck.GetRandomDeck(_previews);

      SelectedCard = Deck.GetPreviewCard() ?? _previews.GetRandomPreview();
    }

    [Updates("DeckName")]
    public virtual Deck Deck { get; protected set; }

    public string DeckName
    {
      get
      {
        var name = Deck.Name;

        if (!_isSaved)
        {
          name = name + "*";
        }
        return name;
      }
    }

    public Library Library { get; private set; }
    public object Dialog { get { return _dialogs.FirstOrDefault(); } }
    public virtual Card SelectedCard { get; protected set; }

    [Updates("Dialog")]
    public virtual void AddDialog(object dialog, DialogType dialogType)
    {
      _dialogs.Add(dialog);
    }

    [Updates("Dialog")]
    public virtual void RemoveDialog(object dialog)
    {
      _dialogs.Remove(dialog);
    }

    public bool HasFocus(object dialog)
    {
      return dialog == Dialog;
    }

    public void CloseAllDialogs()
    {
      foreach (var dialog in _dialogs.ToList())
      {
        dialog.Close();
      }
    }

    public void ChangeSelectedCard(string name)
    {
      SelectedCard = GetCard(name);
    }

    private Card GetCard(string name)
    {
      return _previews[name];
    }

    public void Open()
    {
      if (!SaveChanges())
        return;

      var configuration = new Configuration
        {
          ScreenTitle = "Select a deck to edit",
          ForwardText = "Select",
          PreviousScreen = this,
          Forward = (deck) =>
            {
              Deck = deck;
              _isNewDeck = false;
              _isSaved = true;
              _shell.ChangeScreen(this);
            }
        };

      var selectDeck = _selectDeckScreenFactory.Create(configuration);
      _shell.ChangeScreen(selectDeck);
    }

    public void New()
    {
      if (!SaveChanges())
        return;

      Deck = new Deck(_previews)
        {
          Name = NewDeckName
        };

      _isNewDeck = true;
      _isSaved = true;
    }

    [Updates("Deck", "DeckName")]
    public virtual void Save()
    {
      if (_isNewDeck)
      {
        SaveAs();
        return;
      }

      Deck.Save();

      _isSaved = true;
    }

    [Updates("Deck", "DeckName")]
    public virtual void SaveAs()
    {
      var deckName = GetDeckName();

      if (deckName == null)
        return;

      Deck.Name = deckName;
      Deck.Save();

      _isNewDeck = false;
      _isSaved = true;
    }

    private string GetDeckName()
    {
      var dialog = _saveDeckAsFactory.Create();
      _shell.ShowModalDialog(dialog);

      if (dialog.WasCanceled)
        return null;

      return dialog.DeckName;
    }

    public void Back()
    {
      if (SaveChanges())
      {
        _shell.ChangeScreen(_previousScreen);
      }
    }

    private bool SaveChanges()
    {
      if (!_isSaved)
      {
        var result = _shell.ShowMessageBox("Do you want to save your changes?", MessageBoxButton.YesNoCancel,
          DialogType.Large,
          "Deck was modified");

        if (result == MessageBoxResult.Cancel)
        {
          return false;
        }

        if (result == MessageBoxResult.Yes)
        {
          Save();
        }
      }

      return true;
    }

    [Updates("Deck", "DeckName")]
    public virtual void AddToDeck(string name)
    {
      Deck.AddCard(name);
      _isSaved = false;
    }

    [Updates("Deck", "DeckName")]
    public virtual void RemoveFromDeck(string name)
    {
      Deck.RemoveCard(name);
      _isSaved = false;
    }

    public interface IFactory
    {
      ViewModel Create(IIsDialogHost previousScreen);
    }
  }
}