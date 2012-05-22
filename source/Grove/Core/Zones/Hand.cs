namespace Grove.Core.Zones
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  public interface IHandQuery : IEnumerable<Card>
  {
    int Count { get; }
    IEnumerable<Card> Lands { get; }
  }

  public class Hand : UnorderedZone, IHandQuery
  {
    public Hand(ChangeTracker changeTracker) : base(changeTracker) {}

    private Hand()
    {
      /* for state copy */
    }

    public bool CanMulligan { get { return Count >= 1; } }
    public int MulliganSize { get { return Count - 1; } }

    public int Score { get { return this.Sum(x => x.Score); } }
    public override Zone Zone { get { return Zone.Hand; } }

    public void Discard()
    {
      Clear();
    }
  }
}