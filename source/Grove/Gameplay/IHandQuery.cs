namespace Grove.Gameplay
{
  using System.Collections.Generic;

  public interface IHandQuery : IZoneQuery
  {
    IEnumerable<Card> Lands { get; }
  }
}