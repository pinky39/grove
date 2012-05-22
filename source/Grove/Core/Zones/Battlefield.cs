namespace Grove.Core.Zones
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  public interface IBattlefieldQuery : IEnumerable<Card>
  {
    IEnumerable<Card> Attackers { get; }
    IEnumerable<Card> Blockers { get; }
    IEnumerable<Card> Creatures { get; }
    IEnumerable<Card> CreaturesThatCanAttack { get; }
    IEnumerable<Card> CreaturesThatCanBlock { get; }
    bool HasCreaturesThatCanAttack { get; }
    IEnumerable<Card> Lands { get; }
    IEnumerable<Card> Legends { get; }
  }


  public class Battlefield : UnorderedZone, IBattlefieldQuery
  {
    public Battlefield(ChangeTracker changeTracker) : base(changeTracker) {}

    private Battlefield()
    {
      /* for state copy */
    }

    public int Score { get { return this.Sum(x => x.Score); } }

    public override Zone Zone { get { return Zone.Battlefield; } }

    public IEnumerable<Card> Attackers { get { return this.Where(card => card.IsAttacker); } }
    public IEnumerable<Card> Blockers { get { return this.Where(card => card.IsBlocker); } }
    public IEnumerable<Card> CreaturesThatCanAttack { get { return Creatures.Where(x => x.CanAttack); } }
    public IEnumerable<Card> CreaturesThatCanBlock { get { return Creatures.Where(x => x.CanBlock()); } }
    public bool HasCreaturesThatCanAttack { get { return this.Any(card => card.CanAttack); } }
    public IEnumerable<Card> Legends { get { return this.Where(x => x.Is().Legendary); } }
  }

  public static class BattlefieldEx
  {
    public static IEnumerable<Card> Permanents(this Players players)
    {
      return players.SelectMany(x => x.Battlefield);
    }
  }
}