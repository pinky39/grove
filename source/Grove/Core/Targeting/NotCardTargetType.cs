namespace Grove.Core.Targeting
{
  using Details.Cards;

  public class NotCardTargetType : ITargetType
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

    public bool OfType(string type)
    {
      return false;
    }
  }
}