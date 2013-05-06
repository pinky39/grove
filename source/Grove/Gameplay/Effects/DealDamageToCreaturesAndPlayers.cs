namespace Grove.Gameplay.Effects
{
  using System;
  using Damage;

  public class DealDamageToCreaturesAndPlayers : Effect
  {
    private readonly Func<Effect, Card, int> _amountCreature;
    private readonly Func<Effect, Player, int> _amountPlayer;
    private readonly Func<Effect, Card, bool> _filterCreature;
    private readonly Func<Effect, Player, bool> _filterPlayer;

    private DealDamageToCreaturesAndPlayers() {}

    public DealDamageToCreaturesAndPlayers(
      int amountCreature = 0,
      int amountPlayer = 0,
      Func<Effect, Card, bool> filterCreature = null,
      Func<Effect, Player, bool> filterPlayer = null) :
        this(delegate { return amountCreature; }, delegate { return amountPlayer; }, filterCreature, filterPlayer) {}

    public DealDamageToCreaturesAndPlayers(
      Func<Effect, Card, int> amountCreature = null,
      Func<Effect, Player, int> amountPlayer = null,
      Func<Effect, Card, bool> filterCreature = null,
      Func<Effect, Player, bool> filterPlayer = null)
    {
      _amountCreature = amountCreature ?? delegate { return 0; };
      _amountPlayer = amountPlayer ?? delegate { return 0; };
      _filterCreature = filterCreature ?? delegate { return true; };
      _filterPlayer = filterPlayer ?? delegate { return true; };
    }

    private bool ShouldDealToCreature(Card creature)
    {
      return _amountCreature(this, creature) > 0 && _filterCreature(this, creature);
    }

    private bool ShouldDealToPlayer(Player player)
    {
      return _amountPlayer(this, player) > 0 && _filterPlayer(this, player);
    }

    public override int CalculatePlayerDamage(Player player)
    {
      return ShouldDealToPlayer(player) ? _amountPlayer(this, player) : 0;
    }

    public override int CalculateCreatureDamage(Card creature)
    {
      return ShouldDealToCreature(creature) ? _amountCreature(this, creature) : 0;
    }

    protected override void ResolveEffect()
    {
      foreach (var player in Players)
      {
        if (ShouldDealToPlayer(player))
        {
          var damage = new Damage(
            source: Source.OwningCard,
            amount: _amountPlayer(this, player),
            isCombat: false,
            changeTracker: Game.ChangeTracker
            );

          player.DealDamage(damage);
        }
      }

      foreach (var player in Players)
      {
        foreach (var creature in player.Battlefield.Creatures)
        {
          if (ShouldDealToCreature(creature))
          {
            var damage = new Damage(
              source: Source.OwningCard,
              amount: _amountCreature(this, creature),
              isCombat: false,
              changeTracker: Game.ChangeTracker
              );

            creature.DealDamage(damage);
          }
        }
      }
    }
  }
}