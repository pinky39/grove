namespace Grove.Core.Zones
{
  using System.Collections.Generic;
  using System.Linq;

  public class Battlefield : UnorderedZone, IBattlefieldQuery
  {
    private readonly Combat _combat;

    public Battlefield(Game game) : base(game)
    {
      _combat = game.Combat;
    }

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

    protected override void AfterAdd(Card card)
    {
      card.HasSummoningSickness = true;
    }

    protected override void AfterRemove(Card card)
    {
      _combat.Remove(card);
      card.DetachAttachments();
      card.Detach();
      card.Untap();
      card.ClearDamage();
    }
  }
}