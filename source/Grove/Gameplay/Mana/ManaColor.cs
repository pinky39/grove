namespace Grove.Gameplay.Mana
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class ManaColor : IEquatable<ManaColor>
  {
    public static readonly ManaColor White = new ManaColor(isWhite: true);
    public static readonly ManaColor Any = new ManaColor(isWhite: true, isBlue: true, isBlack: true, isRed: true, isGreen: true);
    public static readonly ManaColor Blue = new ManaColor(isBlue: true);
    public static readonly ManaColor Black = new ManaColor(isBlack: true);
    public static readonly ManaColor Red = new ManaColor(isRed: true);
    public static readonly ManaColor Green = new ManaColor(isGreen: true);
    public static readonly ManaColor Colorless = new ManaColor(isColorless: true);
    
    private readonly List<int> _colorIndices = new List<int>(6);
    private readonly bool[] _isColor;

    public ManaColor(bool isWhite = false, bool isBlue = false, bool isBlack = false, bool isRed = false,
      bool isGreen = false, bool isColorless = false)
    {
      _isColor = new[] {isWhite, isBlue, isBlack, isRed, isGreen, isColorless};

      for (var i = 0; i < _isColor.Length; i++)
      {
        if (_isColor[i])
        {
          _colorIndices.Add(i);
        }
      }
    }

    public List<int> Indices { get { return _colorIndices; } }
    
    public bool IsWhite { get { return _isColor[0]; } }
    public bool IsBlue { get { return _isColor[1]; } }
    public bool IsBlack { get { return _isColor[2]; } }
    public bool IsRed { get { return _isColor[3]; } }
    public bool IsGreen { get { return _isColor[4]; } }
    public bool IsColorless { get { return _isColor[5]; } }
    public bool IsMulti { get { return _colorIndices.Count > 1; } }

    public bool Equals(ManaColor other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;

      return _colorIndices.SequenceEqual(other._colorIndices);            
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != typeof (ManaColor)) return false;
      return Equals((ManaColor) obj);
    }

    public override int GetHashCode()
    {
      var hash = 0;
      
      foreach (var colorIndex in _colorIndices)
      {
        hash = hash ^ colorIndex*397;
      }

      return hash;
    }

    public static bool operator ==(ManaColor left, ManaColor right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(ManaColor left, ManaColor right)
    {
      return !Equals(left, right);
    }
  }
}