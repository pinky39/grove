namespace Grove.Gameplay
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Text;

  public class ScenarioGenerator
  {
    private const string ScenarioTemplate =
      @"
      [Fact]
      public void Scenario#()
      {        
        [P1Hand]
        [P2Hand]
        [P1Battlefield]
        [P2Battlefield]        
      }    
    ";

    private readonly string _fileName;
    private readonly Game _game;
    private int _scenarioNumber = 1;

    public ScenarioGenerator(Game game)
    {
      _game = game;
      _fileName = String.Format("generated-scenario-{0}.txt", Guid.NewGuid());
    }

    public void WriteScenario()
    {
      using (var writer = new StreamWriter(_fileName, append: true))
      {
        writer.WriteLine(CreateScenario());
        _scenarioNumber++;
      }
    }

    public string WriteScenarioToString()
    {
      return CreateScenario();
    }

    private static string CreateBattlefield(IEnumerable<Card> cards, int playerId)
    {
      var sb = new StringBuilder();
      sb.AppendFormat("Battlefield(P{0}, ", playerId);

      foreach (var card in cards)
      {
        if (card.AttachedTo != null)
          continue;

        if (card.HasAttachments)
        {
          sb.AppendFormat("C({0})", CreateCard(card));

          foreach (var attachedCard in card.Attachments)
          {
            if (attachedCard.Is().Enchantment)
            {
              sb.AppendFormat(".IsEnchantedWith({0})", CreateCard(attachedCard));
              continue;
            }

            sb.AppendFormat(".IsEquipedWith({0})", CreateCard(attachedCard));
          }

          sb.Append(", ");
          continue;
        }

        sb.AppendFormat("{0}, ", CreateCard(card));
      }

      sb.Remove(sb.Length - 2, 2);
      sb.Append(");");

      return sb.ToString();
    }

    private static string CreateCard(Card card)
    {
      return String.Format("\"{0}\"", card.Name);
    }

    private static string CreateHand(IEnumerable<Card> cards, int playerId)
    {
      var sb = new StringBuilder();
      sb.AppendFormat("Hand(P{0}, ", playerId);

      foreach (var card in cards)
      {
        sb.AppendFormat("{0}, ", CreateCard(card));
      }

      sb.Remove(sb.Length - 2, 2);
      sb.Append(");");

      return sb.ToString();
    }

    private string CreateScenario()
    {
      var scenario = new StringBuilder(ScenarioTemplate);

      scenario = scenario.Replace("Scenario#", "Scenario" + _scenarioNumber);
      scenario = scenario.Replace("[P1Hand]", CreateHand(_game.Players.Player1.Hand, 1));
      scenario = scenario.Replace("[P2Hand]", CreateHand(_game.Players.Player2.Hand, 2));
      scenario = scenario.Replace("[P1Battlefield]", CreateBattlefield(_game.Players.Player1.Battlefield, 1));
      scenario = scenario.Replace("[P2Battlefield]", CreateBattlefield(_game.Players.Player2.Battlefield, 2));

      return scenario.ToString();
    }
  }
}