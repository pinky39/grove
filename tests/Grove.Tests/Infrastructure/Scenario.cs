namespace Grove.Tests.Infrastructure
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using Core;
  using Core.Ai;
  using Core.Controllers;
  using Core.Controllers.Scenario;
  using Core.Zones;
  using log4net.Config;
  using Xunit;

  public abstract class Scenario : IDisposable
  {
    protected static readonly IoC Container = IoC.Test();
    private readonly IDisposable _scope;

    protected Scenario(bool player1ControlledByScript = true, bool player2ControlledByScript = true)
    {
      _scope = Container.BeginScope();
      CreatePlayers(player1ControlledByScript, player2ControlledByScript);
    }

    protected CardDatabase CardDatabase
    {
      get { return Container.Resolve<CardDatabase>(); }
    }

    protected DecisionFactory DecisionFactory
    {
      get { return Container.Resolve<DecisionFactory>(); }
    }

    protected Game Game
    {
      get { return Container.Resolve<Game>(); }
    }

    protected Player P1
    {
      get { return (Player)Game.Players.Player1; }
    }

    protected Player P2
    {
      get { return (Player)Game.Players.Player2; }
    }

    protected Search Search
    {
      get { return Container.Resolve<Search>(); }
    }

    protected Combat Combat
    {
      get { return Game.Combat; }
    }

    private static Player.IFactory PlayerFactory
    {
      get { return Container.Resolve<Player.IFactory>(); }
    }

    public void Dispose()
    {
      _scope.Dispose();

      OnDispose();
    }

    protected virtual void OnDispose() {}

    protected StepDecisions At(Step step, int turn = 1)
    {
      var stepDecisions = Container.Resolve<StepDecisions>();

      stepDecisions.Step = step;
      stepDecisions.Turn = turn;

      return stepDecisions;
    }

    protected void Battlefield(Player player, params ScenarioCard[] cards)
    {
      foreach (var scenarioCard in cards)
      {
        scenarioCard.Initialize(name =>
          {
            var card = CardDatabase.CreateCard(name, player);

            if (card.IsManaSource)
              player.AddManaSources(card.ManaSources);
                        
            player.PutCardToBattlefield(card);
            card.HasSummoningSickness = false;
            
            if (scenarioCard.IsTapped)
              card.Tap();

            foreach (var enchantment in scenarioCard.Enchantments)
            {
              enchantment.Initialize(enchantmentName =>
                {
                  var enchantmentCard = CardDatabase.CreateCard(enchantmentName, player);
                  player.PutCardToBattlefield(enchantmentCard);
                  EnchantCard(card, enchantmentCard);
                  return enchantmentCard;
                });
            }

            foreach (var equipment in scenarioCard.Equipments)
            {
              equipment.Initialize(equipmentName =>
                {
                  var equipmentCard = CardDatabase.CreateCard(equipmentName, player);
                  player.PutCardToBattlefield(equipmentCard);
                  EquipCard(card, equipmentCard);
                  return equipmentCard;
                });
            }

             foreach (var tracked in scenarioCard.TrackedBy)
            {
              tracked.Initialize(trackerName =>
                {
                  var tracker = CardDatabase.CreateCard(trackerName, player);
                  player.PutCardToBattlefield(tracker);
                  TrackCard(card, tracker);
                  return tracker;
                });
            }

            return card;
          });
      }
    }

    protected IEnumerable<Card> Permanents(Player controller, params string[] cardNames)
    {
      foreach (var cardName in cardNames)
      {
        var battlefield = (Battlefield) controller.Battlefield;
        var card = CardDatabase.CreateCard(cardName, controller);
        battlefield.Add(card);
        yield return card;
      }
    }

    protected ScenarioCard C(string name)
    {
      return name;
    }

    protected Card C(ScenarioCard scenarioCard)
    {
      return scenarioCard;
    }

    protected ScenarioEffect E(ScenarioCard scenarioCard)
    {
      return new ScenarioEffect {Effect = () => Game.Stack.First(x => x.Source.OwningCard == scenarioCard.Card)};
    }

    protected void EnableLogging()
    {
      var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(
        "Grove.Tests.Infrastructure.Logger.Debug.xml");

      XmlConfigurator.Configure(stream);
    }


    protected void Equal<T>(T expected, T actual)
    {
      Assert.Equal(expected, actual);
    }

    protected void EquipCard(Card card, Card equipment)
    {
      card.EquipWithoutPayingTheCost(equipment);
    }

    protected void TrackCard(Card card, Card tracker)
    {
      card.Attach(tracker);
    }

    protected void Exec(params StepDecisions[] decisions)
    {
      const int untilTurn = 5;
      DecisionFactory.AddDecisions(decisions);
      RunGame(untilTurn);
      AssertAllCommandsHaveRun(decisions);
    }

    protected void False(bool condition, string message = null)
    {
      Assert.False(condition, message);
    }

    protected void Graveyard(Player player, params ScenarioCard[] cards)
    {
      var graveyard = (Graveyard) player.Graveyard;

      foreach (var scenarioCard in cards)
      {
          scenarioCard.Initialize(name =>
          {
            var card = CardDatabase.CreateCard(name, player);
            graveyard.Add(card);

            if (card.IsManaSource)
              player.AddManaSources(card.ManaSources);

            return card;
          });
      }
    }
    
    protected void Hand(Player player, params ScenarioCard[] cards)
    {
      var hand = (Hand) player.Hand;

      foreach (var scenarioCard in cards)
      {
        scenarioCard.Initialize(name =>
          {
            var card = CardDatabase.CreateCard(name, player);
            hand.Add(card);

            if (card.IsManaSource)
              player.AddManaSources(card.ManaSources);

            return card;
          });
      }
    }

    protected void Null(object obj)
    {
      Assert.Null(obj);
    }

    protected virtual void RunGame(int maxTurnCount)
    {
      Game.Start(maxTurnCount, skipPreGame: true);
    }

    protected void True(bool condition, string message = null)
    {
      Assert.True(condition, message);
    }

    protected void UnEquipCard(Card card, Card equipment)
    {
      card.Detach(equipment);
    }

    private static void AssertAllCommandsHaveRun(IEnumerable<StepDecisions> commands)
    {
      foreach (var stepCommands in commands)
      {
        stepCommands.AssertAllWereExecuted();
      }
    }

    private static void EnchantCard(Card card, Card enchantment)
    {
      card.EnchantWithoutPayingTheCost(enchantment);
    }

    private void CreatePlayers(bool p1IsControlledByScript, bool p2IsControlledByScript)
    {
      Game.Players.Player1 = PlayerFactory.Create(
        name: "Player 1",
        avatar: String.Empty,
        type: p1IsControlledByScript ? PlayerType.Scenario : PlayerType.Computer,
        deck: Deck.Dummy());

      Game.Players.Player2 = PlayerFactory.Create(
        name: "Player 2",
        avatar: String.Empty,
        type: p2IsControlledByScript ? PlayerType.Scenario : PlayerType.Computer,
        deck: Deck.Dummy());


      Game.Players.Starting = Game.Players.Player1;
    }
  }
}