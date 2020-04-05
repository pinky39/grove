namespace Grove.UserInterface.NewTournament
{
  using System;
  using System.Collections.Generic;
  using Media;

  public class ViewModel : ViewModelBase
  {
    private readonly int[] _draftTournamentSize = new[] {4, 6, 8, 10};
    private readonly object _previousScreen;
    private readonly int[] _sealedTournamentSize = new[] {30, 50, 100, 150, 200, 300, 500};
    private readonly List<string> _sets;
    private TournamentType _typeOfTournament;

    public ViewModel(object previousScreen)
    {
      _previousScreen = previousScreen;
      _sets = MediaLibrary.GetSetsNames();

      StarterPack = Settings.Readonly.DefaultStarterPack;
      BoosterPack1 = Settings.Readonly.DefaultBoosterPack1;
      BoosterPack2 = Settings.Readonly.DefaultBoosterPack2;
      BoosterPack3 = Settings.Readonly.DefaultBoosterPack3;
      YourName = "You";
      TypeOfTournament = TournamentType.Sealed;
      TournamentDescription = TournamentDescriptions.Sealed;
    }

    public IEnumerable<string> Sets
    {
      get { return _sets; }
    }

    public virtual int[] TournamentSize { get; protected set; }
    public virtual string TournamentDescription { get; protected set; }

    public virtual int PlayersCount { get; set; }
    public string YourName { get; set; }
    public virtual int Avatar { get; protected set; }
    public string StarterPack { get; set; }
    public string BoosterPack1 { get; set; }
    public string BoosterPack2 { get; set; }
    public string BoosterPack3 { get; set; }

    public virtual TournamentType TypeOfTournament
    {
      get { return _typeOfTournament; }
      set
      {
        _typeOfTournament = value;

        TournamentSize = _typeOfTournament == TournamentType.Sealed ? _sealedTournamentSize : _draftTournamentSize;
        PlayersCount = _typeOfTournament == TournamentType.Sealed ? 200 : 8;
        TournamentDescription = _typeOfTournament == TournamentType.Sealed
          ? TournamentDescriptions.Sealed
          : TournamentDescriptions.Draft;
      }
    }

    public void PreviousAvatar()
    {
      Avatar--;
    }

    public void NextAvatar()
    {
      Avatar++;
    }

    public void Start()
    {
      var p = TournamentParameters.Default(
        YourName,
        Avatar,
        PlayersCount,
        new[] {BoosterPack1, BoosterPack2, BoosterPack3},
        StarterPack,
        TypeOfTournament);

      try
      {
        Ui.Tournament = new Tournament(p);
        Ui.Tournament.Start();
      }
      catch (Exception ex)
      {
        HandleException(ex);
      }

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