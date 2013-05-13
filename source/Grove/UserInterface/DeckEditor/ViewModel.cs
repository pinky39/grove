namespace Grove.UserInterface.DeckEditor
{
  using System.Windows;
  using Gameplay;
  using Infrastructure;
  using SelectDeck;

  public class ViewModel : ViewModelBase, IIsDialogHost
  {
    private readonly object _previousScreen;

    private UserInterface.Deck.ViewModel _deck;

    public ViewModel(object previousScreen)
    {
      _previousScreen = previousScreen;
    }

    public virtual UserInterface.Deck.ViewModel Deck
    {
      get { return _deck; }
      set
      {
        _deck = value;
        _deck.Property(x => x.SelectedCard).Changes(this).Property<ViewModel, Card>(x => x.SelectedCard);
      }
    }

    public LibraryFilter.ViewModel LibraryFilter { get; private set; }
    public object Dialog { get; private set; }
    public virtual Card SelectedCard { get; protected set; }

    [Updates("Dialog")]
    public virtual void AddDialog(object dialog, DialogType dialogType)
    {
      Dialog = dialog;
    }

    [Updates("Dialog")]
    public virtual void RemoveDialog(object dialog)
    {
      Dialog = null;
    }

    public bool HasFocus(object dialog)
    {
      return dialog == Dialog;
    }

    public void CloseAllDialogs()
    {
      if (Dialog != null)
        Dialog.Close();
    }

    public override void Initialize()
    {
      var allCardNames = CardsInfo.GetCardNames();
      LibraryFilter = ViewModels.LibraryFilter.Create(allCardNames, x => x);

      Deck = ViewModels.Deck.Create();
      SelectedCard = CardsInfo[allCardNames[0]];
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
          Forward = deck =>
            {
              Deck = ViewModels.Deck.Create(deck);
              Shell.ChangeScreen(this);
            }
        };

      var selectDeck = ViewModels.SelectDeck.Create(configuration);
      Shell.ChangeScreen(selectDeck);
    }

    public void New()
    {
      if (!SaveChanges())
        return;

      Deck = ViewModels.Deck.Create();
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
      var dialog = ViewModels.SaveDeckAs.Create();
      Shell.ShowModalDialog(dialog);

      if (dialog.WasCanceled)
        return null;

      return dialog.DeckName;
    }

    public void Back()
    {
      if (SaveChanges())
      {
        Shell.ChangeScreen(_previousScreen);
      }
    }

    private bool SaveChanges()
    {
      if (!Deck.IsSaved)
      {
        var result = Shell.ShowMessageBox("Do you want to save your changes?", MessageBoxButton.YesNoCancel,
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
      ViewModel Create(object previousScreen);
    }
  }
}