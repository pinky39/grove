namespace Grove.UserInterface.DraftScreen
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Artifical;
  using Caliburn.Micro;
  using Gameplay;
  using Gameplay.Tournaments;
  using Infrastructure;

  public class ViewModel : ViewModelBase, IDraftCardPicker
  {
    private ThreadBlocker _blocker;
    private readonly BindableCollection<Card> _library = new BindableCollection<Card>();
    private readonly List<TournamentPlayer> _participants = new List<TournamentPlayer>();

    public ViewModel(IEnumerable<TournamentPlayer> participants)
    {
      _participants.AddRange(participants);
    }

    public IEnumerable<TournamentPlayer> Participants { get { return _participants; } }
    public IEnumerable<Card> Library { get { return _library; } }
    
    public virtual IEnumerable<Card> BoosterRow1 { get; protected set; }
    public virtual IEnumerable<Card> BoosterRow2 { get; protected set; }
    public virtual IEnumerable<Card> BoosterRow3 { get; protected set; }

    public virtual int Round { get; protected set; }
    public virtual int CardsLeft { get; protected set; }
    public virtual Card PickedCard { get; set; }
    public virtual Card PreviewCard { get; protected set; }

    public int CreatureCount { get { return Library.Count(x => x.Is().Creature); } }
    public int LandCount { get { return Library.Count(x => x.Is().Land); } }
    public int SpellsCount { get { return Library.Count() - CreatureCount - LandCount; } }

    public CardInfo PickCard(List<CardInfo> draftedCards, List<CardInfo> booster, int round, CardRatings ratings)
    {
      Round = round;
      
      BoosterRow1 = booster.Take(5).Select(x => CardsDictionary[x.Name]).ToList();
      BoosterRow2 = booster.Skip(5).Take(5).Select(x => CardsDictionary[x.Name]).ToList();
      BoosterRow3 = booster.Skip(10).Take(5).Select(x => CardsDictionary[x.Name]).ToList();
      
      CardsLeft = (3 - round)*15 + booster.Count;

      _blocker = new ThreadBlocker();
      _blocker.BlockUntilCompleted();

      return booster.First(x => x.Name.Equals(PickedCard.Name, StringComparison.InvariantCultureIgnoreCase));
    }

    [Updates("CreatureCount", "LandCount", "SpellsCount")]
    public virtual void SelectCard(Card card)
    {
      PickedCard = card;
      _library.Add(card);

      _blocker.Completed();
    }

    public void ShowPreview(Card card)
    {
      PreviewCard = card;
    }

    public interface IFactory
    {
      ViewModel Create(IEnumerable<TournamentPlayer> participants);
    }
  }
}