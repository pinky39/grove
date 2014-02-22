namespace Grove.Gameplay
{
  using Grove.Gameplay.Decisions;
  using Grove.Gameplay.Effects;
  using Grove.Infrastructure;

  public interface ITarget : IHashable
  {
    int Id { get; }
  }

  public static class Target
  {
    public static Zone? Zone(this ITarget target)
    {
      if (target.IsPlayer())
      {
        return null;
      }

      if (target.IsEffect())
      {
        return target.Effect().IsOnStack
          ? Gameplay.Zone.Stack
          : Gameplay.Zone.None;
      }

      return target.Card().Zone;
    }

    public static Card Card(this ITarget target)
    {
      return target as Card;
    }

    public static Player Player(this ITarget target)
    {
      return target as Player;
    }

    public static Effect Effect(this ITarget target)
    {
      var effect = target as Effect;

      if (effect != null)
        return effect;

      var lazyEffect = target as ScenarioEffect;

      return lazyEffect != null ? lazyEffect.Effect() : null;
    }

    public static bool IsCard(this ITarget target)
    {
      return target is Card;
    }

    public static bool IsPlayer(this ITarget target)
    {
      return target is Player;
    }

    public static bool IsEffect(this ITarget target)
    {
      return target is Effect || target is ScenarioEffect;
    }

    public static void ReceiveDamage(this ITarget target, Damage damage)
    {
      var damageable = target as IDamageable;

      if (damageable != null)
      {
        damageable.ReceiveDamage(damage);
      }
    }

    public static int Life(this ITarget target)
    {
      var haslife = target as IHasLife;

      if (haslife != null)
        return haslife.Life;

      return 0;
    }

    public static Player Controller(this ITarget target)
    {
      if (target.IsPlayer())
        return target.Player();

      if (target.IsEffect())
        return target.Effect().Controller;

      return target.Card().Controller;
    }

    public static ITargetType Is(this ITarget target)
    {
      if (target.IsCard())
      {
        return target.Card().Is();
      }

      return new NotCardTargetType();
    }

    private class NotCardTargetType : ITargetType
    {
      public bool Artifact { get { return false; } }

      public bool Attachment { get { return false; } }
      public bool BasicLand { get { return false; } }
      public bool Creature { get { return false; } }
      public bool Enchantment { get { return false; } }
      public bool Equipment { get { return false; } }
      public bool Instant { get { return false; } }
      public bool Land { get { return false; } }
      public bool Legendary { get { return false; } }
      public bool Sorcery { get { return false; } }
      public bool Token { get { return false; } }
      public bool Aura { get { return false; } }
      public bool NonBasicLand { get { return false; } }

      public bool OfType(string type)
      {
        return false;
      }
    }
  }
}