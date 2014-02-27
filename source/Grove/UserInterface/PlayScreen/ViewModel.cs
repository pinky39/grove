namespace Grove.UserInterface.PlayScreen
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using Debug;
  using Events;
  using Infrastructure;

  public class ViewModel : ViewModelBase, IIsDialogHost, IReceive<BeforeSpellWasPutOnStack>,
    IReceive<BeforeActivatedAbilityWasPutOnStack>,
    IReceive<SearchStarted>, IReceive<SearchFinished>, IReceive<DamageHasBeenDealt>,
    IReceive<AssignedCombatDamageWasDealt>, IReceive<CardWasRevealed>, IReceive<PlayerHasFlippedACoin>,
    IReceive<EffectOptionsWereChosen>, IReceive<TurnStarted>, IReceive<ZoneChanged>, IReceive<PlayerLifeChanged>,
    IDisposable
  {
    private readonly List<object> _largeDialogs = new List<object>();
    private readonly List<object> _smallDialogs = new List<object>();

    private ScenarioGenerator _scenarioGenerator;

    public object LargeDialog { get { return _largeDialogs.FirstOrDefault(); } }
    public MagnifiedCard.ViewModel MagnifiedCard { get; set; }
    public ManaPool.ViewModel ManaPool { get; set; }
    public UserInterface.Battlefield.ViewModel OpponentsBattlefield { get; private set; }
    public PlayerBox.ViewModel You { get; private set; }
    public PlayerBox.ViewModel Opponent { get; private set; }
    public virtual string SearchInProgressMessage { get; set; }
    public object SmallDialog { get { return _smallDialogs.FirstOrDefault(); } }
    public UserInterface.Stack.ViewModel StackVm { get; set; }
    public Steps.ViewModel Steps { get; set; }
    public TurnNumber.ViewModel TurnNumber { get; set; }
    public MessageLog.ViewModel MessageLog { get; set; }
    public UserInterface.Battlefield.ViewModel YourBattlefield { get; private set; }
    public UserInterface.Zones.ViewModel Zones { get; set; }
    public virtual QuitGame.ViewModel QuitGameDialog { get; protected set; }

    public void Dispose()
    {
      ManaPool.Dispose();
      YourBattlefield.Dispose();
      OpponentsBattlefield.Dispose();
      Zones.Dispose();
      You.Dispose();
      Opponent.Dispose();
    }

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

    public void Receive(BeforeActivatedAbilityWasPutOnStack message)
    {
      ShowActivationDialog(message);
      MessageLog.AddMessage(message.ToString());
    }

    public void Receive(BeforeSpellWasPutOnStack message)
    {
      ShowActivationDialog(message);
      MessageLog.AddMessage(message.ToString());
    }

    public void Receive(CardWasRevealed message)
    {
      MessageLog.AddMessage(message.ToString());
    }

    public void Receive(DamageHasBeenDealt message)
    {
      MessageLog.AddMessage(message.ToString());
    }

    public void Receive(EffectOptionsWereChosen message)
    {
      MessageLog.AddMessage(message.Text);
    }

    public void Receive(PlayerHasFlippedACoin message)
    {
      MessageLog.AddMessage(message.ToString());
    }

    public void Receive(PlayerLifeChanged message)
    {
      MessageLog.AddMessage(message.ToString());
    }

    public void Receive(SearchFinished message)
    {
      SearchInProgressMessage = null;
    }

    public void Receive(SearchStarted message)
    {
      SearchInProgressMessage = String.Empty;
      TaskUtils.Delay(500).ContinueWith((t) =>
        {
          if (SearchInProgressMessage == String.Empty)
            SearchInProgressMessage = ThinkingMessages.GetRandom();
        });
    }

    public void Receive(TurnStarted message)
    {
      var dialog = ViewModels.NextTurn.Create();
      Shell.ShowDialog(dialog);

      // sync delay
      Thread.Sleep(1500);
      dialog.Close();
    }

    public void Receive(ZoneChanged message)
    {
      if (message.DisplayInformationInUi())
      {
        MessageLog.AddMessage(message.ToString());
      }
    }

    public override void Initialize()
    {
      _scenarioGenerator = new ScenarioGenerator(Game);
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

    private void ShowActivationDialog(ICardActivationMessage activation)
    {
      if (activation.Controller.IsHuman)
        return;

      var dialog = ViewModels.EffectActivation.Create(activation);
      Shell.ShowDialog(dialog);
      Thread.Sleep(2000);
      dialog.Close();
    }

    public interface IFactory
    {
      ViewModel Create();
    }
  }
}