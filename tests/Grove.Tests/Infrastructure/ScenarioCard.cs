namespace Grove.Tests.Infrastructure
{
  using System;
  using System.Collections.Generic;
  using Grove.Core;
  using Grove.Infrastructure;

  [Copyable]
  public class ScenarioCard
  {
    private readonly List<ScenarioCard> _enchantments = new List<ScenarioCard>();
    private readonly List<ScenarioCard> _equipments = new List<ScenarioCard>();
    private readonly string _name;

    private ScenarioCard() {}
    
    public ScenarioCard(string name)
    {
      _name = name;
    }

    public IEnumerable<ScenarioCard> Enchantments { get { return _enchantments; } }
    public IEnumerable<ScenarioCard> Equipments { get { return _equipments; } }
    public bool IsTapped { get; private set; }    
    public Card Card { get; private set; }        

    public int CalculateHash(HashCalculator hashCalculator)
    {
      return Card.CalculateHash(hashCalculator);
    }
    
    public void Initialize(Func<string, Card> cardFactory)
    {
      Card = cardFactory(_name);
    }

    public ScenarioCard IsEnchantedWith(params ScenarioCard[] enchantments)
    {
      _enchantments.AddRange(enchantments);
      return this;
    }

    public ScenarioCard IsEquipedWith(params ScenarioCard[] equipments)
    {
      _equipments.AddRange(equipments);
      return this;
    } 

    public ScenarioCard Tap()
    {
      IsTapped = true;
      return this;
    }


    public static implicit operator ScenarioCard(string cardName)
    {
      return new ScenarioCard(cardName);
    }

    public static implicit operator Card(ScenarioCard scenarioCard)
    {
      return scenarioCard.Card;
    }
  }
}