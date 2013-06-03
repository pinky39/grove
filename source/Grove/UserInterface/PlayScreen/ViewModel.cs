namespace Grove.UserInterface.PlayScreen
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using Gameplay.Debuging;
  using Gameplay.Messages;
  using Infrastructure;

  public class ViewModel : ViewModelBase, IIsDialogHost, IReceive<PlayerHasCastASpell>,
    IReceive<PlayerHasActivatedAbility>,
    IReceive<SearchStarted>, IReceive<SearchFinished>, IReceive<DamageHasBeenDealt>,
    IReceive<AssignedCombatDamageWasDealt>, IReceive<CardWasRevealed>, IReceive<PlayerHasFlippedACoin>
  {
    private readonly List<object> _largeDialogs = new List<object>();
    private readonly List<object> _smallDialogs = new List<object>();
    private ScenarioGenerator _scenarioGenerator;

    public object LargeDialog { get { return _largeDialogs.FirstOrDefault(); } }
    public MagnifiedCard.ViewModel MagnifiedCard { get; set; }
    public ManaPool.ViewModel ManaPool { get; set; }
    public Battlefield.ViewModel OpponentsBattlefield { get; private set; }
    public PlayerBox.ViewModel You { get; private set; }
    public PlayerBox.ViewModel Opponent { get; private set; }
    public virtual string SearchInProgressMessage { get; set; }
    public object SmallDialog { get { return _smallDialogs.FirstOrDefault(); } }
    public Stack.ViewModel StackVm { get; set; }
    public Turn.ViewModel TurnVm { get; set; }
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

    public void Receive(CardWasRevealed message)
    {
      MessageLog.AddMessage(message.ToString());
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

    public void Receive(PlayerHasFlippedACoin message)
    {
      MessageLog.AddMessage(message.ToString());
    }

    public void Receive(SearchFinished message)
    {
      SearchInProgressMessage = null;
    }

    public void Receive(SearchStarted message)
    {
      SearchInProgressMessage = string.Format("Counting mammoths ({0},{1})... ",
        message.SearchDepthLimit, message.TargetCountLimit);
    }

    public override void Initialize()
    {
      _scenarioGenerator = new ScenarioGenerator(CurrentGame);
      OpponentsBattlefield = ViewModels.Battlefield.Create(Players.Computer);
      YourBattlefield = ViewModels.Battlefield.Create(Players.Human);
      You = ViewModels.PlayerBox.Create(Players.Human);
      Opponent = ViewModels.PlayerBox.Create(Players.Computer);
    }

    public void GenerateTestScenario()
    {
      _scenarioGenerator.WriteScenario();
    }

    public void QuitGame()
    {
      var dialog = ViewModels.QuitGame.Create();
      ((IClosable) dialog).Closed += delegate { QuitGameDialog = null; };

      QuitGameDialog = dialog;
    }

    public interface IFactory
    {
      ViewModel Create();
    }
  }
}