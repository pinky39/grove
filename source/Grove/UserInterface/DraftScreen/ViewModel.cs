namespace Grove.UserInterface.DraftScreen
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Caliburn.Micro;
  using Gameplay;
  using Gameplay.Tournaments;
  using Infrastructure;

  public class ViewModel : ViewModelBase, IDraftingStrategy
  {
    private readonly BindableCollection<Card> _library = new BindableCollection<Card>();
    private readonly List<TournamentPlayer> _participants = new List<TournamentPlayer>();
    private ThreadBlocker _blocker;

    public ViewModel(IEnumerable<TournamentPlayer> participants)
    {
      _participants.AddRange(participants.Rotate(
        RandomEx.Next(participants.Count())));
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
    public virtual string Direction { get; protected set; }
    public bool PlayerLeftDraft { get; private set; }

    public int CreatureCount { get { return Library.Count(x => x.Is().Creature); } }
    public int LandCount { get { return Library.Count(x => x.Is().Land); } }
    public int SpellsCount { get { return Library.Count() - CreatureCount - LandCount; } }

    public CardInfo PickCard(List<CardInfo> booster, int round)
    {
      Round = round;

      BoosterRow1 = CreateCards(booster.Take(5)).ToList();
      BoosterRow2 = CreateCards(booster.Skip(5).Take(5)).ToList();
      BoosterRow3 = CreateCards(booster.Skip(10).Take(5)).ToList();
      Direction = round == 2 ? "Up" : "Down";

      CardsLeft = (3 - round)*15 + booster.Count;

      _blocker = new ThreadBlocker();
      _blocker.BlockUntilCompleted();

      if (PlayerLeftDraft)
        return null;
      
      return booster.First(x => x.Name.Equals(PickedCard.Name, StringComparison.InvariantCultureIgnoreCase));
    }

    private IEnumerable<Card> CreateCards(IEnumerable<CardInfo> infos)
    {
      return infos.Select(x =>
        {
          var card = CardsDatabase.CreateCard(x.Name);
          card.Rarity = x.Rarity;
          card.Set = x.Set;

          return card;
        });
    }

    [Updates("CreatureCount", "LandCount", "SpellsCount")]
    public virtual void SelectCard(Card card)
    {
      PickedCard = card;
      _library.Add(card);

      _blocker.Completed();
    }

    public void Quit()
    {
      PlayerLeftDraft = true;
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