namespace Grove
{
  using System.Collections.Generic;
  using System.Linq;

  public class Battlefield : UnorderedZone, IBattlefieldQuery
  {
    public Battlefield(Player owner) : base(owner) {}

    private Battlefield()
    {
      /* for state copy */
    }

    public int Score { get { return this.Sum(x => x.Score); } }


    public override Zone Name { get { return Zone.Battlefield; } }
    public IEnumerable<Card> Attackers { get { return Creatures.Where(card => card.IsAttacker); } }
    public IEnumerable<Card> Blockers { get { return Creatures.Where(card => card.IsBlocker); } }
    public IEnumerable<Card> CreaturesThatCanAttack { get { return Creatures.Where(x => x.CanAttack); } }
    public IEnumerable<Card> CreaturesThatCanBlock { get { return Creatures.Where(x => x.CanBlock()); } }
    public bool HasCreaturesThatCanAttack { get { return Creatures.Any(card => card.CanAttack); } }
    public IEnumerable<Card> Legends { get { return this.Where(x => x.Is().Legendary); } }
    public IEnumerable<Card> Planewalkers { get { return this.Where(x => x.Is().Planeswalker); } }

    public IEnumerable<CardColor> PermanentsColors
    {
      get
      {
        return this          
          .SelectMany(x => x.Colors)
          .Where(x => x != CardColor.Colorless && x != CardColor.None)
          .Distinct();
      }
    }    
  }
}