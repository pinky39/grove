namespace Grove
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
    bool Aura { get; }
    bool NonBasicLand { get; }
  }

  public class CardType : ITargetType
  {
    private static readonly HashSet<string> BasicTypes = new HashSet<string>(new[]
      {        
        "artifact",
        "land",
        "enchantment",
        "instant",
        "land",
        "plane",
        "planeswalker",
        "scheme",
        "sorcery",
        "tribal",
        "vanguard",
        "token",
        "creature"
      });

    private static readonly HashSet<string> BasicLandTypes = new HashSet<string>(new[]
      {
        "plains",
        "island",
        "swamp",
        "mountain",
        "forest",
      });

    private string _display;

    private bool _isArtifact;
    private bool _isAura;
    private bool _isBasicLand;
    private bool _isCreature;
    private bool _isEnchantment;
    private bool _isEquipment;
    private bool _isInstant;
    private bool _isLand;
    private bool _isLegendary;
    private bool _isSorcery;
    private bool _isToken;

    private HashSet<string> _mainTypes = new HashSet<string>();
    private HashSet<string> _subTypes = new HashSet<string>();
    private HashSet<string> _superTypes = new HashSet<string>();

    private CardType(HashSet<string> superTypes, HashSet<string> mainTypes, HashSet<string> subTypes)
    {
      _superTypes = superTypes;
      _mainTypes = mainTypes;
      _subTypes = subTypes;

      Init();
    }

    public CardType(string typeString)
    {
      typeString = typeString.ToLowerInvariant();
      var types = ParseTypeString(typeString);
      var active = _superTypes;

      foreach (var type in types)
      {
        if (BasicTypes.Contains(type))
        {
          active = _subTypes;
          _mainTypes.Add(type);
        }
        else
        {
          active.Add(type);
        }
      }

      Init();
    }


    public IEnumerable<string> MainTypes
    {
      get { return _mainTypes; }
    }

    public IEnumerable<string> SubTypes
    {
      get { return _subTypes; }
    }

    public IEnumerable<string> SuperTypes
    {
      get { return _superTypes; }
    }

    public static CardType None
    {
      get { return new CardType(String.Empty); }
    }

    public bool Artifact
    {
      get { return _isArtifact; }
    }

    public bool Attachment
    {
      get { return Aura || Equipment; }
    }

    public bool BasicLand
    {
      get { return _isBasicLand; }
    }

    public bool Creature
    {
      get { return _isCreature; }
    }

    public bool Enchantment
    {
      get { return _isEnchantment; }
    }

    public bool Equipment
    {
      get { return _isEquipment; }
    }

    public bool Instant
    {
      get { return _isInstant; }
    }

    public bool Land
    {
      get { return _isLand; }
    }

    public bool Legendary
    {
      get { return _isLegendary; }
    }

    public bool Sorcery
    {
      get { return _isSorcery; }
    }

    public bool Token
    {
      get { return _isToken; }
    }

    public bool Aura
    {
      get { return _isAura; }
    }

    public bool NonBasicLand
    {
      get { return Land && !BasicLand; }
    }

    private void Init()
    {
      var superAndMain = String.Join(" ", _superTypes
        .OrderBy(x => x)
        .Concat(_mainTypes.OrderBy(x => x))
        .Select(x => x.Capitalize()));

      if (_subTypes.Count > 0)
      {
        var sub = String.Join(" ", _subTypes
          .OrderBy(x => x)
          .Select(x => x.Capitalize()));

        _display = String.Format("{0} — {1}", superAndMain, sub);
      }
      else
      {
        _display = superAndMain;
      }

      _isCreature = Is("creature");
      _isLand = Is("land");
      _isBasicLand = Is("basic land");
      _isLegendary = Is("legendary");
      _isArtifact = Is("artifact");
      _isEnchantment = Is("enchantment");
      _isEquipment = Is("equipment");
      _isAura = Is("aura");
      _isInstant = Is("instant");
      _isSorcery = Is("sorcery");
      _isToken = Is("token");
    }

    private static string[] ParseTypeString(string typeString)
    {
      var types = typeString.Split(
        new[] {' ', '-', '—'},
        StringSplitOptions.RemoveEmptyEntries);
      return types;
    }

    public bool Is(string typeString)
    {
      var types = ParseTypeString(typeString);

      foreach (var type in types)
      {
        if (_superTypes.Contains(type))
          continue;

        if (_mainTypes.Contains(type))
          continue;

        if (_subTypes.Contains(type))
          continue;

        return false;
      }

      return true;
    }

    public bool IsAny(params string[] types)
    {
      return IsAny(types.AsEnumerable());
    }

    public bool IsAny(IEnumerable<string> types)
    {
      return types.Any(Is);
    }

    public override string ToString()
    {
      return _display;
    }

    public static implicit operator CardType(string cardTypes)
    {
      return new CardType(cardTypes);
    }

    public CardType ReplaceLandTypeWith(string type)
    {
      var superTypes = new HashSet<string>(_superTypes);
      var subTypes = new HashSet<string>(_subTypes);
      var mainTypes = new HashSet<string>(_mainTypes);

      subTypes.ExceptWith(BasicLandTypes);
      superTypes.Add(type);

      return new CardType(superTypes, mainTypes, subTypes);
    }

    public CardType AddBasicLandType(string type)
    {
      var superTypes = new HashSet<string>(_superTypes);
      var subTypes = new HashSet<string>(_subTypes);
      var mainTypes = new HashSet<string>(_mainTypes);

      if (!superTypes.Contains(type))
      {
        superTypes.Add(type);
      }

      return new CardType(superTypes, mainTypes, subTypes);
    }
  }
}