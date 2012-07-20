namespace Grove.Core.Zones
{
  using System.Collections.Generic;

  public interface IHandQuery : IEnumerable<Card>
  {
    int Count { get; }
    IEnumerable<Card> Lands { get; }
  }
}