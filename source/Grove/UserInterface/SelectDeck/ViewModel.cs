namespace Grove.UserInterface.SelectDeck
{
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using Castle.Core;
  using Gameplay;
  using Infrastructure;
  using Persistance;
  using Shell;

  public class ViewModel : IIsDialogHost
  {
    private readonly CardDatabase _cardDatabase;
    private readonly Configuration _configuration;
    private readonly UserInterface.Deck.ViewModel.IFactory _deckVmFactory;
    private readonly List<UserInterface.Deck.ViewModel> _decks = new List<UserInterface.Deck.ViewModel>();
    private readonly IShell _shell;

    public ViewModel(CardDatabase cardDatabase, IShell shell, Configuration configuration,
      UserInterface.Deck.ViewModel.IFactory deckVmFactory)
    {
      _cardDatabase = cardDatabase;
      _shell = shell;
      _configuration = configuration;
      _deckVmFactory = deckVmFactory;

      LoadDecks();
    }

    [DoNotWire]
    public virtual UserInterface.Deck.ViewModel Selected { get; set; }

    public string NextCaption { get { return _configuration.ForwardText; } }

    public string Title { get { return _configuration.ScreenTitle; } }

    public List<UserInterface.Deck.ViewModel> Decks { get { return _decks; } }

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