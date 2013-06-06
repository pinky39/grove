namespace Grove.UserInterface.NewTournament
{
  using System.Collections.Generic;
  using Gameplay.Tournaments;

  public class ViewModel : ViewModelBase
  {
    private readonly object _previousScreen;
    private readonly List<string> _sets;
    private readonly int[] _tournamentSize = new[] {25, 50, 100, 150, 200, 300 };

    public ViewModel(object previousScreen)
    {
      _previousScreen = previousScreen;
      _sets = MediaLibrary.GetSetsNames();

      StarterPack = _sets[0];
      BoosterPack1 = _sets[0];
      BoosterPack2 = _sets[0];
      BoosterPack3 = _sets[0];
      PlayersCount = 50;
      YourName = "You";
    }

    public IEnumerable<string> Sets { get { return _sets; } }
    public IEnumerable<int> TournamentSize { get { return _tournamentSize; } }

    public int PlayersCount { get; set; }
    public string YourName { get; set; }
    public string StarterPack { get; set; }
    public string BoosterPack1 { get; set; }
    public string BoosterPack2 { get; set; }
    public string BoosterPack3 { get; set; }

    public void Start()
    {
      var p = TournamentParameters.Default(
        YourName,
        PlayersCount,
        new[] {BoosterPack1, BoosterPack2, BoosterPack3},
        StarterPack);

      TournamentRunner.Start(p);

      Shell.ChangeScreen(_previousScreen);
    }

    public void Back()
    {
      Shell.ChangeScreen(_previousScreen);
    }

    public interface IFactory
    {
      ViewModel Create(object previousScreen);
    }
  }
}