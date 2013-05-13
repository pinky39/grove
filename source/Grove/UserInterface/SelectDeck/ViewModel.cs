namespace Grove.UserInterface.SelectDeck
{
  using System.Collections.Generic;
  using System.Linq;
  using Castle.Core;
  using Infrastructure;

  public class ViewModel : ViewModelBase
  {
    private readonly Configuration _configuration;
    private readonly List<Deck.ViewModel> _decks = new List<Deck.ViewModel>();

    public ViewModel(Configuration configuration)
    {
      _configuration = configuration;
    }

    [DoNotWire]
    public virtual Deck.ViewModel Selected { get; set; }

    public string NextCaption { get { return _configuration.ForwardText; } }

    public string Title { get { return _configuration.ScreenTitle; } }

    public List<Deck.ViewModel> Decks { get { return _decks; } }

    [Updates("CanStart")]
    public virtual bool IsStarting { get; protected set; }

    public virtual bool CanStart { get { return !IsStarting; } }

    public override void Initialize()
    {
      LoadDecks();
    }

    private void LoadDecks()
    {
      var deckFiles = MediaLibrary.GetDeckFilenames();

      foreach (var fileName in deckFiles)
      {
        _decks.Add(ViewModels.Deck.Create(
          DeckIo.Read(fileName),
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
      Shell.ChangeScreen(_configuration.PreviousScreen);
    }

    public interface IFactory
    {
      ViewModel Create(Configuration configuration);
    }
  }
}