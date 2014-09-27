namespace Grove
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using Grove.Infrastructure;

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

    bool OfType(string type);    
  }

  public class CardType : ITargetType, IEnumerable<string>
  {
    private static readonly string[] BasicTypes = new[]
      {
        "legendary",
        "artifact",
        "basic land",
        "enchantment",
        "instant",
        "land",
        "plane",
        "planeswalker",
        "scheme",
        "sorcery",
        "tribal",
        "vanguard",
        "creature"
      };

    private string[] _basicTypes;

    private bool _isArtifact;
    private bool _isAura;
    private bool _isBasicLand;
    private bool _isCreature;
    private bool _isEnchantment;
    private bool _isEquipment;
    private bool _isLand;
    private bool _isLegendary;
    private string[] _subTypes;
    private string _displayString;
    private bool _isInstant;
    private bool _isSorcery;
    private bool _isToken;

    public CardType(string typeString)
    {
      Initialize(typeString);      
    }

    public static CardType None { get { return new CardType(String.Empty); } }
    public bool Artifact { get { return _isArtifact; } }
    public bool Attachment { get { return Aura || Equipment; } }
    public bool BasicLand { get { return _isBasicLand; } }
    public bool Creature { get { return _isCreature; } }
    public bool Enchantment { get { return _isEnchantment; } }
    public bool Equipment { get { return _isEquipment; } }
    public bool Instant { get { return _isInstant; } }
    public bool Land { get { return _isLand; } }
    public bool Legendary { get { return _isLegendary; } }
    public bool Sorcery { get { return _isSorcery; } }
    public bool Token { get { return _isToken; } }
    public bool Aura { get { return _isAura; } }
    public bool NonBasicLand { get { return Land && !BasicLand; } }

    public bool OfType(string type)
    {
      return Is(type);
    }

    public bool Is(string type)
    {
      if (_basicTypes.Any(x => x.Equals(type, StringComparison.OrdinalIgnoreCase)))
        return true;

      if (_subTypes.Any(x => x.Equals(type, StringComparison.OrdinalIgnoreCase)))
        return true;

      return false;
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

    public string[] Subtypes
    {
      get { return _subTypes; }
    }

    public IEnumerator<string> GetEnumerator()
    {
      foreach (var basicType in _basicTypes)
      {
        yield return basicType;
      }

      foreach (var subType in _subTypes)
      {
        yield return subType;
      }
    }

    public override string ToString()
    {
      return _displayString;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    private void Initialize(string types)
    {
      var basicTypes = new List<string>();
      var displayString = new StringBuilder();
            
      foreach (var basicType in BasicTypes)
      {
        var index = types.IndexOf(basicType, StringComparison.OrdinalIgnoreCase);

        if ((index != -1) && IsWholeWord(types, basicType, index))
        {                              
          if (basicType.Equals("basic land", StringComparison.OrdinalIgnoreCase))
          {
            basicTypes.Add("land");
          }

          basicTypes.Add(basicType);          
          types = types.Remove(index, basicType.Length);

          displayString.Append(basicType.CapitalizeEachWord());
          displayString.Append(" ");
        }
      }

      _basicTypes = basicTypes.ToArray();
      _subTypes = types.Split(new[] { ' ', '-', '—' }, StringSplitOptions.RemoveEmptyEntries);
      
      if (_subTypes.Length != 0)
      {
        displayString.Append("— ");
        displayString.Append(String.Join(" ", _subTypes));
      }

      _displayString = displayString.ToString().Trim();

      InitializeCommonTypes();
    }

    private bool IsWholeWord(string types, string basicType, int index)
    {
      return ( (index == 0 || types[index - 1] == ' ') && (index + basicType.Length == types.Length || types[index + basicType.Length] == ' ') );
    }

    private void InitializeCommonTypes()
    {
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

    public static implicit operator CardType(string cardTypes)
    {
      return new CardType(cardTypes);
    }   

    public CardType ReplaceBasicLandTypeWith(string basicLandType)
    {
      return ToString()
        .Replace("Forest", basicLandType)
        .Replace("Mountain", basicLandType)
        .Replace("Plains", basicLandType)
        .Replace("Island", basicLandType)
        .Replace("Swamp", basicLandType);
    }

    public CardType AddBasicLandType(string basicLanfType)
    {
        var type = ToString();

        if (type.Contains(basicLanfType))
            return type;

        type += " " + basicLanfType;

        return type;
    }
  }
}