namespace Grove.Ui.SelectDeck
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using Core;
  using Infrastructure;
  using Shell;  

  public class Configuration
  {
    public string ScreenTitle { get; set; }
    public string ForwardText { get; set; }
    public Action<Deck> Forward { get; set; }
    public IIsDialogHost PreviousScreen { get; set; }
  }
  
  public class ViewModel : IIsDialogHost
  {
    private readonly CardDatabase _cardDatabase;
    private readonly List<Deck> _decks = new List<Deck>();            
    private readonly Configuration _configuration;
    private readonly IShell _shell;
    

    private Deck _selected;

    public ViewModel(CardDatabase cardDatabase, IShell shell, Configuration configuration)
    {
      _cardDatabase = cardDatabase;      
      _shell = shell;            
      _configuration = configuration;

      LoadDecks();
    }

    public virtual Deck Selected
    {
      get { return _selected; }
      set
      {
        _selected = value;
        SelectedCard = _selected.GetPreviewCard();
      }
    }    

    public string NextCaption
    {
      get { return _configuration.ForwardText; }
    }

    public string Title
    {
      get { return _configuration.ScreenTitle; }
    }

    public virtual Card SelectedCard { get; protected set; }
    public IEnumerable<Deck> Decks { get { return _decks; } }

    [Updates("CanStart")]
    public virtual bool IsStarting { get; protected set; }

    public virtual bool CanStart { get { return !IsStarting; } }
    public void AddDialog(object dialog, DialogType dialogType) {}
    public void RemoveDialog(object dialog) {}

    public bool HasFocus(object dialog)
    {
      return false;
    }

    public void CloseAllDialogs() {}

    private void LoadDecks()
    {
      var deckFiles = Directory.EnumerateFiles(MediaLibrary.DecksFolder, "*.dec");
      var previews = _cardDatabase.CreatePreviewForEveryCard();
      
      foreach (var fileName in deckFiles)
      {
        _decks.Add(new Deck(fileName, previews));
      }

      Selected = _decks[0];
    }

    public void ChangeSelectedCard(string name)
    {
      SelectedCard = Selected.GetCard(name);
    }

    public void Forward()
    {
      
      IsStarting = true;
      
      _configuration.Forward(Selected);
      
      IsStarting = false;            
    }

    public void Back()
    {
      _shell.ChangeScreen(_configuration.PreviousScreen);
    }

    public interface IFactory
    {
      ViewModel Create(Configuration configuration);
    }
  }
}