namespace Grove.Ui.PlayScreen
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Messages;
  using Core.Testing;
  using Infrastructure;
  using Shell;

  public class ViewModel : IIsDialogHost, IReceive<PlayerHasCastASpell>, IReceive<PlayerHasActivatedAbility>,
    IReceive<SearchStarted>, IReceive<SearchFinished>
  {
    private readonly List<object> _largeDialogs = new List<object>();
    private readonly Match _match;
    private readonly List<object> _notifications = new List<object>();
    private readonly ScenarioGenerator _scenarioGenerator;
    private readonly IShell _shell;

    private readonly List<object> _smallDialogs = new List<object>();

    public ViewModel(IShell shell, Players players, Battlefield.ViewModel.IFactory battlefieldFactory,
                     ScenarioGenerator scenarioGenerator, Match match)
    {
      _shell = shell;
      _scenarioGenerator = scenarioGenerator;
      _match = match;

      OpponentsBattlefield = battlefieldFactory.Create(players.Computer);
      YourBattlefield = battlefieldFactory.Create(players.Human);
    }

    public object LargeDialog { get { return _largeDialogs.FirstOrDefault(); } }
    public MagnifiedCard.ViewModel MagnifiedCard { get; set; }
    public ManaPool.ViewModel ManaPool { get; set; }
    public virtual object Notification { get { return _notifications.FirstOrDefault(); } }
    public Battlefield.ViewModel OpponentsBattlefield { get; private set; }
    public Ui.Players.ViewModel PlayersBox { get; set; }
    public virtual bool SearchInProgress { get; set; }
    public object SmallDialog { get { return _smallDialogs.FirstOrDefault(); } }
    public Stack.ViewModel Stack { get; set; }
    public Turn.ViewModel Turn { get; set; }
    public Battlefield.ViewModel YourBattlefield { get; private set; }
    public Zones.ViewModel Zones { get; set; }

    [Updates("SmallDialog", "Notification", "LargeDialog")]
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

        case (DialogType.Notification):
          {
            _notifications.Insert(0, dialog);
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

    [Updates("SmallDialog", "Notification", "LargeDialog")]
    public virtual void RemoveDialog(object dialog)
    {
      _smallDialogs.Remove(dialog);
      _notifications.Remove(dialog);
      _largeDialogs.Remove(dialog);
    }

    public void Receive(PlayerHasActivatedAbility message)
    {
      if (message.HasTarget)
      {
        _shell.ShowNotification(
          String.Format("{0} find(s) that {2} is a great target for {1}.", message.Ability.Controller, message.Ability,
            message.Target));
        return;
      }

      _shell.ShowNotification(
        String.Format("{0} think(s) {1} might resolve the issue.", message.Ability.Controller, message.Ability));
    }


    public void Receive(PlayerHasCastASpell message)
    {
      if (message.HasTarget)
      {
        _shell.ShowNotification(
          String.Format("{0} find(s) that {2} is a great target for {1}.", message.Spell.Controller, message.Spell,
            message.Target));
        return;
      }

      _shell.ShowNotification(
        String.Format("{0} think(s) {1} will solve this mess.", message.Spell.Controller, message.Spell));
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

    public interface IFactory
    {
      ViewModel Create();
    }
  }
}