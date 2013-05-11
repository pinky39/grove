namespace Grove.UserInterface.NewTournament
{
  using System.Collections.Generic;
  using Gameplay;
  using Shell;

  public class ViewModel : IIsDialogHost
  {
    private readonly IIsDialogHost _previousScreen;
    private readonly IShell _shell;
    private readonly Tournament _tournament;
    private readonly List<string> _sets;
    private readonly int[] _tournamentSize = new[] {25, 50, 75, 100, 125};

    public ViewModel(IIsDialogHost previousScreen, IShell shell, Tournament tournament)
    {
      _previousScreen = previousScreen;
      _shell = shell;
      _tournament = tournament;
      _sets = MediaLibrary.GetSetsNames();

      StarterPack = _sets[0];
      BoosterPack1 = _sets[0];
      BoosterPack2 = _sets[0];
      BoosterPack3 = _sets[0];
      PlayersCount = 50;
      YourName = "You";
    }

    public IEnumerable<string> Sets {get { return _sets; }}
    public IEnumerable<int> TournamentSize {get { return _tournamentSize; }}

    public int PlayersCount { get; set; }
    public string YourName { get; set; }
    public string StarterPack { get; set; }
    public string BoosterPack1 { get; set; }
    public string BoosterPack2 { get; set; }
    public string BoosterPack3 { get; set; }

    public void AddDialog(object dialog, DialogType dialogType) {}

    public void RemoveDialog(object dialog) {}

    public bool HasFocus(object dialog)
    {
      return false;
    }

    public void CloseAllDialogs() {}

    public void Start()
    {
      _tournament.Start(
        YourName, 
        PlayersCount, 
        new[] {BoosterPack1, BoosterPack2, BoosterPack3}, 
        StarterPack);
    }

    public void Back()
    {
      _shell.ChangeScreen(_previousScreen);
    }

    public interface IFactory
    {
      ViewModel Create(IIsDialogHost previousScreen);
    }
  }
}