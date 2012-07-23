﻿namespace Grove.Core.Zones
{
  using System.Collections.Generic;

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
}