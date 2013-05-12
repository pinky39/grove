namespace Grove.UserInterface.DeckEditor
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Windows;
  using Gameplay;
  using Infrastructure;
  using SelectDeck;

  public class ViewModel : ViewModelBase, IIsDialogHost
  {
    private readonly CardDatabase _cardDatabase;
    private readonly List<object> _dialogs = new List<object>();
    private readonly IIsDialogHost _previousScreen;

    public ViewModel(IIsDialogHost previousScreen, CardDatabase cardDatabase)
    {
      _previousScreen = previousScreen;
      _cardDatabase = cardDatabase;
      Library = Bindable.Create<Library>(cardDatabase);
    }

    public virtual UserInterface.Deck.ViewModel Deck { get; set; }
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

    public override void Initialize()
    {
      Deck = ViewModels.Deck.Create(MediaLibrary.GetRandomDeck(_cardDatabase));
      SelectedCard = Deck.SelectedCard ?? _cardDatabase.Random();

      Deck.Property(x => x.SelectedCard).Changes(this).Property(x => x.SelectedCard);
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
      ViewModel Create(IIsDialogHost previousScreen);
    }
  }
}