namespace Grove.Gameplay.Targeting
{
  using Card;
  using Card.Characteristics;
  using Common;
  using Damage;
  using Decisions.Scenario;
  using Effects;
  using Gameplay.Zones;
  using Infrastructure;
  using Modifiers;
  using Player;

  public interface ITarget : IHashable {}

  public static class Target
  {
    public static void AddModifier(this ITarget target, IModifier modifier)
    {
      var acceptsModifiers = target as IAcceptsModifiers;
      if (acceptsModifiers != null)
      {
        acceptsModifiers.AddModifier(modifier);
      }
    }

    public static Zone? Zone(this ITarget target)
    {
      if (target.IsPlayer())
      {
        return null;
      }

      if (target.IsEffect())
      {
        return Gameplay.Zones.Zone.Stack;
      }

      return target.Card().Zone;
    }

    public static void RemoveModifier(this ITarget target, IModifier modifier)
    {
      var acceptsModifiers = target as IAcceptsModifiers;
      if (acceptsModifiers != null)
      {
        acceptsModifiers.RemoveModifier(modifier);
      }
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

    public static void DealDamage(this ITarget target, Damage damage)
    {
      var damageable = target as IDamageable;

      if (damageable != null)
      {
        damageable.DealDamage(damage);
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