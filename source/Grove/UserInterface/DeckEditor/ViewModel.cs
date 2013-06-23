namespace Grove.UserInterface.DeckEditor
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Windows;
  using Gameplay;
  using Infrastructure;
  using SelectDeck;

  public class ViewModel : ViewModelBase
  {
    private readonly object _previousScreen;
    private Dictionary<string, LibraryItem> _libraryItems;

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

        _deck.SelectedCardChanged += delegate { SelectedCard = CardsDictionary[_deck.SelectedCard.Name]; };
      }
    }

    public LibraryFilter.ViewModel LibraryFilter { get; private set; }    
    public virtual Card SelectedCard { get; protected set; }    

    public override void Initialize()
    {      
      _libraryItems = CardsDictionary
        .GetCardNames()
        .Select(x => new LibraryItem
          {
            Card = CardsDictionary[x],
            Info = new CardInfo(x)
          })
        .ToDictionary(x => x.Info.Name, x => x);

      LibraryFilter = ViewModels.LibraryFilter.Create(
        cards: _libraryItems.Values.Select(x => x.Info), 
        transformResult:  x => _libraryItems[x.Name],
        orderBy: x => 0);

      Deck = ViewModels.Deck.Create();
      SelectedCard = CardsDictionary[_libraryItems.First().Value.Info.Name];
    }

    public void ChangeSelectedCard(LibraryItem libraryItem)
    {
      SelectedCard = libraryItem.Card;
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

    public void AddCard(LibraryItem libraryItem)
    {
      Deck.AddCard(libraryItem.Info);
    }

    public void RemoveCard(LibraryItem libraryItem)
    {
      Deck.RemoveCard(libraryItem.Info);
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