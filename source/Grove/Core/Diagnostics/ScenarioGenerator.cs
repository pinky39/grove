namespace Grove.Diagnostics
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Text;

  public static class ScenarioGenerator
  {
    private const string ScenarioTemplate =
@"
[Fact]
public void Scenario()
{{        
{0}   
}}
";

    public static void WriteScenario(Game game)
    {
      var fileName = String.Format("generated-scenario-{0}.txt", Guid.NewGuid());
      using (var writer = new StreamWriter(fileName, append: true))
      {
        writer.WriteLine(WriteScenarioToString(game));
      }
    }

    public static string WriteScenarioToString(Game game)
    {            
      var inner = new StringBuilder();

      inner = inner.AppendLine(CreateZone("Hand", game.Players.Player1.Hand, 1));
      inner = inner.AppendLine(CreateZone("Hand", game.Players.Player2.Hand, 2));
      inner = inner.AppendLine(CreateBattlefield(game.Players.Player1.Battlefield, 1));
      inner = inner.AppendLine(CreateBattlefield(game.Players.Player2.Battlefield, 2));
      inner = inner.AppendLine(CreateZone("Graveyard", game.Players.Player1.Graveyard, 1));
      inner = inner.AppendLine(CreateZone("Graveyard", game.Players.Player2.Graveyard, 2));
      inner = inner.AppendLine(CreateZone("Library", game.Players.Player1.Library, 1));
      inner = inner.AppendLine(CreateZone("Library", game.Players.Player2.Library, 2));      
      inner = inner.AppendLine(String.Format("P1.Life={0};", game.Players.Player1.Life));
      inner = inner.AppendLine(String.Format("P2.Life={0};", game.Players.Player2.Life));
      inner = inner.AppendLine("RunGame(1);");
      
      return String.Format(ScenarioTemplate, inner);
    }    
    
    private static string CreateBattlefield(IEnumerable<Card> cards, int player)
    {
      var sb = new StringBuilder();
      sb.AppendFormat("Battlefield(P{0}, ", player);

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

    private static string CreateZone(string zone, IEnumerable<Card> cards, int player)
    {
      var sb = new StringBuilder();
      sb.AppendFormat("{0}(P{1}, ", zone, player);

      foreach (var card in cards)
      {
        sb.AppendFormat("{0}, ", CreateCard(card));
      }

      sb.Remove(sb.Length - 2, 2);
      sb.Append(");");

      return sb.ToString();
    }
  }
}