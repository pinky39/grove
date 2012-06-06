namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  public interface ITargetType
  {
    bool Artifact { get; }
    bool Attachment { get; }
    bool BasicLand { get; }
    bool Creature { get; }
    bool Enchantment { get; }
    bool Equipment { get; }
    bool Instant { get; }
    bool Land { get; }
    bool Legendary { get; }    
    bool Sorcery { get; }
    bool Token { get; }

    bool OfType(string type);
  }

  public class CardType : ITargetType
  {
    private static readonly List<string> BasicTypes = new List<string>{
      "artifact", "enchantment", "instant", "land", "plane", "planeswalker",
      "scheme", "sorcery", "tribal", "vanguard", "basic land", "creature"
    };

    private readonly HashSet<string> _map;
    private readonly string _string;
    private bool _isCreature;
    private bool _isLand;
    private bool _isLegendary;

    private CardType(IEnumerable<string> types)
    {
      _map = new HashSet<string>(types);
      _string = CreateTypeString(types);

      InitializeCommonTypes();
    }

    public CardType(string typeString) : this(ParseTypes(typeString)) {}


    public static CardType None { get { return new CardType(String.Empty); } }
    public bool Artifact { get { return Is("artifact"); } }
    public bool Attachment { get { return Enchantment || Equipment; } }
    public bool BasicLand { get { return Is("basic land"); } }
    public bool Creature { get { return _isCreature; } }
    public bool Enchantment { get { return Is("enchantment"); } }
    public bool Equipment { get { return Is("equipment"); } }
    public bool Instant { get { return Is("instant"); } }
    public bool Land { get { return _isLand; } }
    public bool Legendary { get { return _isLegendary; } }    
    public bool Sorcery { get { return Is("sorcery"); } }
    public bool Token { get { return Is("token"); } }

    public bool OfType(string type)
    {
      return Is(type);
    }

    public bool Is(string type)
    {
      if (string.IsNullOrEmpty(type))
        throw new ArgumentException("Type should not be empty.");

      var types = type.Split(' ');

      if (types.Length == 1)
      {
        return _map.Contains(types[0]);
      }

      var subtypes = new HashSet<string>(types);
      return subtypes.IsSubsetOf(_map);
    }

    public bool IsAny(params string[] types)
    {
      return IsAny(types.AsEnumerable());
    }

    public bool IsAny(IEnumerable<string> types)
    {
      foreach (var type in types)
      {
        if (Is(type))
          return true;
      }

      return false;
    }

    public override string ToString()
    {
      return _string;
    }

    private static string CreateTypeString(IEnumerable<string> types)
    {
      var basic = types
        .Where(BasicTypes.Contains)
        .OrderBy(BasicTypes.IndexOf)
        .Select(x => x.Capitalize());

      var other = types
        .Where(x => !BasicTypes.Contains(x))
        .OrderBy(x => x)
        .Select(x => x.Capitalize())
        .ToList();

      if (other.Count == 0)
      {
        return string.Join(" ", basic);
      }
      
      return String.Format("{0} — {1}",
        string.Join(" ", basic),
        string.Join(" ", other));
    }

    private static List<string> ParseTypes(string spellTypes)
    {
      return new List<string>(spellTypes.Split(new[]{' ', '-'},
        StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLower()));
    }

    private void InitializeCommonTypes()
    {
      _isCreature = Is("creature");
      _isLand = Is("land");
      _isLegendary = Is("legendary");
    }

    public static implicit operator CardType(string cardTypes)
    {
      return new CardType(cardTypes);
    }

    public static CardType operator +(CardType left, CardType right)
    {
      var types = new HashSet<string>();
      types.UnionWith(left._map);
      types.UnionWith(right._map);

      return new CardType(types);
    }
  }
}