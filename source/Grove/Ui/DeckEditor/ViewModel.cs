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
    private readonly Ui.Deck.ViewModel.IFactory _deckVmFactory;
    private readonly List<object> _dialogs = new List<object>();
    private readonly IIsDialogHost _previousScreen;
    private readonly SaveDeckAs.ViewModel.IFactory _saveDeckAsFactory;
    private readonly SelectDeck.ViewModel.IFactory _selectDeckScreenFactory;
    private readonly IShell _shell;

    private Ui.Deck.ViewModel _deck;

    public ViewModel(
      IIsDialogHost previousScreen, IShell shell,
      SelectDeck.ViewModel.IFactory selectDeckScreenFactory,
      SaveDeckAs.ViewModel.IFactory saveDeckAsFactory,
      Ui.Deck.ViewModel.IFactory deckVmFactory,
      CardDatabase cardDatabase)
    {
      _previousScreen = previousScreen;
      _shell = shell;
      _selectDeckScreenFactory = selectDeckScreenFactory;
      _saveDeckAsFactory = saveDeckAsFactory;
      _deckVmFactory = deckVmFactory;

      Library = Bindable.Create<Library>(cardDatabase);
      Deck = deckVmFactory.Create(MediaLibrary.GetRandomDeck(cardDatabase));
      SelectedCard = Deck.SelectedCard ?? cardDatabase.Random();
    }

    public virtual Ui.Deck.ViewModel Deck
    {
      get { return _deck; }
      protected set
      {
        _deck = value;
        _deck.Property(x => x.SelectedCard).Changes(this).Property<ViewModel, Card>(x => x.SelectedCard);
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

    public void ChangeSelectedCard(Card card)
    {
      SelectedCard = card;
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
              Deck = _deckVmFactory.Create(deck);
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

      Deck = _deckVmFactory.Create();
    }

    public void Save()
    {
      if (Deck.IsNew)
      {
        SaveAs();
        return;
      }

      Deck.Save();
    }

    public virtual void SaveAs()
    {
      var deckName = GetDeckName();

      if (deckName == null)
        return;

      Deck.SaveAs(deckName);
    }

    public void AddCard(Card card)
    {
      Deck.AddCard(card.Name);
    }

    public void RemoveCard(Card card)
    {
      Deck.RemoveCard(card.Name);
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
      if (!Deck.IsSaved)
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

    public interface IFactory
    {
      ViewModel Create(IIsDialogHost previousScreen);
    }
  }
}