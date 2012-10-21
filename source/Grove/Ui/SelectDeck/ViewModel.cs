namespace Grove.Ui.SelectDeck
{
  using System.Collections.Generic;
  using System.IO;
  using Core;
  using Infrastructure;
  using Shell;

  public class ViewModel : IIsDialogHost
  {
    private readonly CardDatabase _cardDatabase;
    private readonly List<Deck> _decks = new List<Deck>();            
    private readonly Configuration _configuration;
    private readonly IShell _shell;
    

    private Deck _selected;

    public ViewModel(CardDatabase cardDatabase, IShell shell, Configuration configuration, CardPreviews previews)
    {
      _cardDatabase = cardDatabase;      
      _shell = shell;            
      _configuration = configuration;

      LoadDecks(previews);
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

    private void LoadDecks(CardPreviews previews)
    {
      var deckFiles = Directory.EnumerateFiles(MediaLibrary.DecksFolder, "*.dec");      
      
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