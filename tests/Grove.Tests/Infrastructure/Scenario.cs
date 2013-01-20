namespace Grove.Tests.Infrastructure
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using Core;
  using Core.Ai;
  using Core.Decisions;
  using Core.Decisions.Scenario;
  using Core.Zones;
  using log4net.Config;
  using Xunit;

  public abstract class Scenario : IDisposable
  {
    protected static readonly IoC Container = IoC.Test();

    protected Scenario(bool player1ControlledByScript = true, bool player2ControlledByScript = true)
    {
      var player1Controller = player1ControlledByScript ? ControllerType.Scenario : ControllerType.Machine;
      var player2Controller = player2ControlledByScript ? ControllerType.Scenario : ControllerType.Machine;

      Game = Game.NewScenario(player1Controller, player2Controller, CardDatabase, DecisionSystem);
    }

    protected CardDatabase CardDatabase { get { return Container.Resolve<CardDatabase>(); } }
    protected DecisionSystem DecisionSystem { get { return Container.Resolve<DecisionSystem>(); } }

    protected Game Game { get; private set; }
    protected Player P1 { get { return Game.Players.Player1; } }
    protected Player P2 { get { return Game.Players.Player2; } }
    protected Search Search { get { return Game.Search; } }
    protected Combat Combat { get { return Game.Combat; } }

    public virtual void Dispose() {}

    protected DecisionsForOneStep At(Step step, int turn = 1)
    {
      return new DecisionsForOneStep(step, turn, Game);
    }

    protected void Library(Player player, params ScenarioCard[] cards)
    {
      var library = (Library) player.Library;

      foreach (var scenarioCard in cards)
      {
        scenarioCard.Initialize(name =>
          {
            var card = CardDatabase.CreateCard(name, player, Game);
            library.Add(card);

            if (card.IsManaSource)
              player.AddManaSources(card.ManaSources);

            return card;
          });
      }
    }

    protected void Battlefield(Player player, params ScenarioCard[] cards)
    {
      foreach (var scenarioCard in cards)
      {
        scenarioCard.Initialize(name =>
          {
            var card = CardDatabase.CreateCard(name, player, Game);

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
                  var enchantmentCard = CardDatabase.CreateCard(enchantmentName, player, Game);
                  player.PutCardToBattlefield(enchantmentCard);
                  EnchantCard(card, enchantmentCard);
                  return enchantmentCard;
                });
            }

            foreach (var equipment in scenarioCard.Equipments)
            {
              equipment.Initialize(equipmentName =>
                {
                  var equipmentCard = CardDatabase.CreateCard(equipmentName, player, Game);
                  player.PutCardToBattlefield(equipmentCard);
                  EquipCard(card, equipmentCard);
                  return equipmentCard;
                });
            }

            foreach (var tracked in scenarioCard.TrackedBy)
            {
              tracked.Initialize(trackerName =>
                {
                  var tracker = CardDatabase.CreateCard(trackerName, player, Game);
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
        var card = CardDatabase.CreateCard(cardName, controller, Game);
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
      equipment.EquipWithoutPayingCost(card);
    }

    protected void TrackCard(Card card, Card tracker)
    {
      card.Attach(tracker);
    }

    protected void Exec(params DecisionsForOneStep[] decisions)
    {
      const int untilTurn = 5;
      Game.AddScenarioDecisions(decisions);
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
            var card = CardDatabase.CreateCard(name, player, Game);
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
            var card = CardDatabase.CreateCard(name, player, Game);
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

    private static void AssertAllCommandsHaveRun(IEnumerable<DecisionsForOneStep> commands)
    {
      foreach (var stepCommands in commands)
      {
        stepCommands.AssertAllWereExecuted();
      }
    }

    private static void EnchantCard(Card card, Card enchantment)
    {
      card.EnchantWithoutPayingCost(enchantment);
    }
  }
}