namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using Infrastructure;

  public class Mana : IEquatable<Mana>
  {
    private readonly ManaColors _colors;

    public Mana(ManaColors colors = ManaColors.Colorless)
    {
      _colors = colors;
    }

    public static Mana Any
    {
      get { return new Mana(ManaColors.White | ManaColors.Blue | ManaColors.Black | ManaColors.Red | ManaColors.Green); }
    }

    public static Mana Black
    {
      get { return new Mana(ManaColors.Black); }
    }

    public static Mana Blue
    {
      get { return new Mana(ManaColors.Blue); }
    }

    public ManaColors Colors
    {
      get { return _colors; }
    }

    public static Mana Green
    {
      get { return new Mana(ManaColors.Green); }
    }

    public bool IsColored
    {
      get { return !IsColorless; }
    }

    public bool IsColorless
    {
      get { return _colors == ManaColors.Colorless; }
    }

    public bool IsMultiColor
    {
      get { return Rank > 1; }
    }

    public int Order
    {
      get { return (int) _colors; }
    }

    public int Rank
    {
      get { return EnumEx.GetSetBitCount((long) _colors); }
    }

    public static Mana Red
    {
      get { return new Mana(ManaColors.Red); }
    }

    public string Symbol
    {
      get { return ManaAmount.GetSymbolFromColor(_colors); }
    }

    public static Mana White
    {
      get { return new Mana(ManaColors.White); }
    }

    #region IEquatable<Mana> Members

    public bool Equals(Mana other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return Equals(other._colors, _colors);
    }

    #endregion

    public IEnumerable<ManaColors> EnumerateColors()
    {
      if (HasColor(ManaColors.White))
        yield return ManaColors.White;

      if (HasColor(ManaColors.Blue))
        yield return ManaColors.Blue;

      if (HasColor(ManaColors.Black))
        yield return ManaColors.Black;

      if (HasColor(ManaColors.Red))
        yield return ManaColors.Red;

      if (HasColor(ManaColors.Green))
        yield return ManaColors.Green;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != typeof (Mana)) return false;
      return Equals((Mana) obj);
    }

    public override int GetHashCode()
    {
      return _colors.GetHashCode();
    }

    public bool HasColor(ManaColors color)
    {
      return (_colors & color) == color;
    }

    public bool IsSingleColor(ManaColors color)
    {
      return HasColor(color) && Rank == 1;
    }

    public override string ToString()
    {
      return String.Format("{{{0}}}", _colors);
    }

    public static bool operator ==(Mana left, Mana right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(Mana left, Mana right)
    {
      return !Equals(left, right);
    }
    
    public IManaAmount ToAmount()
    {
      return new PrimitiveManaAmount(this);
    }
        
  }
}