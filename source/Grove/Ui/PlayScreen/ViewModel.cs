namespace Grove.Ui.PlayScreen
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using Core;
  using Core.Messages;
  using Core.Testing;
  using Infrastructure;

  public class ViewModel : IIsDialogHost, IReceive<PlayerHasCastASpell>, IReceive<PlayerHasActivatedAbility>, 
    IReceive<SearchStarted>, IReceive<SearchFinished>, IReceive<DamageHasBeenDealt>, IReceive<AssignedCombatDamageWasDealt>
  {
    private readonly List<object> _largeDialogs = new List<object>();
    private readonly QuitGame.ViewModel.IFactory _quitGameFactory;
    private readonly ScenarioGenerator _scenarioGenerator;

    private readonly List<object> _smallDialogs = new List<object>();

    public ViewModel(Players players, Battlefield.ViewModel.IFactory battlefieldFactory,
                     ScenarioGenerator scenarioGenerator, QuitGame.ViewModel.IFactory quitGameFactory)
    {
      _scenarioGenerator = scenarioGenerator;
      _quitGameFactory = quitGameFactory;

      OpponentsBattlefield = battlefieldFactory.Create(players.Computer);
      YourBattlefield = battlefieldFactory.Create(players.Human);
    }

    public object LargeDialog { get { return _largeDialogs.FirstOrDefault(); } }
    public MagnifiedCard.ViewModel MagnifiedCard { get; set; }
    public ManaPool.ViewModel ManaPool { get; set; }
    public Battlefield.ViewModel OpponentsBattlefield { get; private set; }
    public Ui.Players.ViewModel PlayersBox { get; set; }
    public virtual bool SearchInProgress { get; set; }
    public object SmallDialog { get { return _smallDialogs.FirstOrDefault(); } }
    public Stack.ViewModel Stack { get; set; }
    public Turn.ViewModel Turn { get; set; }
    public MessageLog.ViewModel MessageLog { get; set; }
    public Battlefield.ViewModel YourBattlefield { get; private set; }
    public Zones.ViewModel Zones { get; set; }
    public virtual QuitGame.ViewModel QuitGameDialog { get; protected set; }

    [Updates("SmallDialog", "LargeDialog")]
    public virtual void AddDialog(object dialog, DialogType dialogType)
    {
      switch (dialogType)
      {
        case (DialogType.Small):
          {
            _smallDialogs.Insert(0, dialog);
            break;
          }

        case (DialogType.Large):
          {
            _largeDialogs.Insert(0, dialog);
            break;
          }
      }
    }

    public bool HasFocus(object dialog)
    {
      if (LargeDialog != null)
      {
        return dialog == LargeDialog;
      }

      return dialog == SmallDialog;
    }

    public void CloseAllDialogs()
    {
      foreach (var largeDialog in _largeDialogs.ToList())
      {        
        largeDialog.Close();
      }

      foreach (var smallDialog in _smallDialogs.ToList())
      {
        smallDialog.Close();
      }
    }

    [Updates("SmallDialog", "LargeDialog")]
    public virtual void RemoveDialog(object dialog)
    {
      _smallDialogs.Remove(dialog);
      _largeDialogs.Remove(dialog);
    }

    public void Receive(AssignedCombatDamageWasDealt message)
    {
      // pause the game a bit after dealing combat damage
      // before creatures go to graveyards
      Thread.Sleep(500);
    }

    public void Receive(DamageHasBeenDealt message)
    {
      MessageLog.AddMessage(message.ToString());
    }

    public void Receive(PlayerHasActivatedAbility message)
    {
      MessageLog.AddMessage(message.ToString());
    }

    public void Receive(PlayerHasCastASpell message)
    {
      MessageLog.AddMessage(message.ToString());
    }  

    public void Receive(SearchFinished message)
    {
      SearchInProgress = false;
    }

    public void Receive(SearchStarted message)
    {
      SearchInProgress = true;
    }

    public void GenerateTestScenario()
    {
      _scenarioGenerator.WriteScenario();
    }

    public void QuitGame()
    {
      var dialog = _quitGameFactory.Create();
      ((IClosable) dialog).Closed += delegate
        {
          QuitGameDialog = null;
        };

      QuitGameDialog = dialog;
    }

    public interface IFactory
    {
      ViewModel Create();
    }
  }
}