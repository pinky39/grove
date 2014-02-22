namespace Grove.UserInterface
{
  using SelectEffectChoice;


  public class Dialogs
  {
    public DamageOrder.ViewModel.IFactory DamageOrder { get; set; }
    public Priority.ViewModel.IFactory Priority { get; set; }
    public ViewModel.IFactory EffectChoiceDialog { get; set; }
    public CardOrder.ViewModel.IFactory CardOrder { get; set; }
    public CombatDamage.ViewModel.IFactory CombatDamage { get; set; }
    public DraftScreen.ViewModel.IFactory DraftScreen { get; set; }
    public DistributeDamage.ViewModel.IFactory DistributeDamage { get; set; }
    public SelectAbility.ViewModel.IFactory SelectAbility { get; set; }
    public SelectTarget.ViewModel.IFactory SelectTarget { get; set; }
    public SelectXCost.ViewModel.IFactory SelectXCost { get; set; }
    public Permanent.ViewModel.IFactory Permanent { get; set; }
    public SelectDeck.ViewModel.IFactory SelectDeck { get; set; }
    public SaveDeckAs.ViewModel.IFactory SaveDeckAs { get; set; }
    public Deck.ViewModel.IFactory Deck { get; set; }
    public SelectableCard.ViewModel.IFactory SelectableCard { get; set; }
    public Spell.ViewModel.IFactory Spell { get; set; }
    public Battlefield.ViewModel.IFactory Battlefield { get; set; }
    public PlayerBox.ViewModel.IFactory PlayerBox { get; set; }
    public QuitGame.ViewModel.IFactory QuitGame { get; set; }
    public StartScreen.ViewModel.IFactory StartScreen { get; set; }
    public DeckEditor.ViewModel.IFactory DeckEditor { get; set; }
    public NewTournament.ViewModel.IFactory NewTournament { get; set; }
    public Step.ViewModel.IFactory Step { get; set; }
    public Hand.ViewModel.IFactory Hand { get; set; }
    public Graveyard.ViewModel.IFactory Graveyard { get; set; }
    public Library.ViewModel.IFactory Library { get; set; }
    public LibraryFilter.ViewModel.IFactory LibraryFilter { get; set; }
    public BuildLimitedDeck.ViewModel.IFactory BuildLimitedDeck { get; set; }
    public GameResults.ViewModel.IFactory GameResults { get; set; }
    public MatchResults.ViewModel.IFactory MatchResults { get; set; }
    public PlayScreen.ViewModel.IFactory PlayScreen { get; set; }
    public Leaderboard.ViewModel.IFactory LeaderBoard { get; set; }
    public LoadSavedGame.ViewModel.IFactory LoadSavedGame { get; set; }
    public NextTurn.ViewModel.IFactory NextTurn { get; set; }
    public CardActivation.ViewModel.IFactory EffectActivation { get; set; }
    public LoadScreen.ViewModel.IFactory LoadScreen { get; set; }        
  }
}