namespace Grove.Ui.SelectDeck
{
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using Castle.Core;
  using Core;
  using Infrastructure;
  using Shell;

  public class ViewModel : IIsDialogHost
  {                
    private readonly Configuration _configuration;
    private readonly Ui.Deck.ViewModel.IFactory _deckVmFactory;
    private readonly CardDatabase _cardDatabase;
    private readonly IShell _shell;        
    private List<Ui.Deck.ViewModel> _decks = new List<Ui.Deck.ViewModel>();

    public ViewModel(CardDatabase cardDatabase, IShell shell, Configuration configuration, Ui.Deck.ViewModel.IFactory deckVmFactory)
    {
      _cardDatabase = cardDatabase;
      _shell = shell;            
      _configuration = configuration;
      _deckVmFactory = deckVmFactory;

      LoadDecks();
    }

    [DoNotWire]
    public virtual Ui.Deck.ViewModel Selected { get; set; }        

    public string NextCaption
    {
      get { return _configuration.ForwardText; }
    }

    public string Title
    {
      get { return _configuration.ScreenTitle; }
    }
    
    public List<Ui.Deck.ViewModel> Decks { get { return _decks; } }

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
      var reader = new DeckReaderWriter();
      
      foreach (var fileName in deckFiles)
      {
        _decks.Add(_deckVmFactory.Create(
          reader.Read(fileName, _cardDatabase), 
          isReadOnly: true));                
      }

      Selected = _decks.FirstOrDefault();
    }
    

    public void Forward()
    {
      
      IsStarting = true;
      
      _configuration.Forward(Selected.Deck);
      
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