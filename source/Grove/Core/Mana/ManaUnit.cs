namespace Grove.Core.Details.Mana
{
  using System;
  using System.Collections.Generic;
  using Infrastructure;

  public class ManaUnit : IEquatable<ManaUnit>
  {
    private readonly ManaColors _colors;    

    public ManaUnit(ManaColors colors = ManaColors.Colorless)
    {
      _colors = colors;      
    }
    
    public static ManaUnit Any { get { return new ManaUnit(ManaColors.White | ManaColors.Blue | ManaColors.Black | ManaColors.Red | ManaColors.Green); } }
    public static ManaUnit Black { get { return new ManaUnit(ManaColors.Black); } }
    public static ManaUnit Blue { get { return new ManaUnit(ManaColors.Blue); } }
    public ManaColors Colors { get { return _colors; } }
    public static ManaUnit Green { get { return new ManaUnit(ManaColors.Green); } }
    public bool IsColored { get { return !IsColorless; } }
    public bool IsColorless { get { return _colors == ManaColors.Colorless; } }
    public bool IsMultiColor { get { return Rank > 1; } }
    public int Order { get { return (int) _colors; } }
    public int Rank { get { return EnumEx.GetSetBitCount((long) _colors); } }
    public static ManaUnit Red { get { return new ManaUnit(ManaColors.Red); } }
    public string Symbol { get { return String.Join(String.Empty, ManaAmount.GetSymbolsFromColor(_colors)); } }
    public static ManaUnit White { get { return new ManaUnit(ManaColors.White); } }

    public bool Equals(ManaUnit other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return Equals(other._colors, _colors);
    }

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
      if (obj.GetType() != typeof (ManaUnit)) return false;
      return Equals((ManaUnit) obj);
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
      return String.Format("{{{0}}}", ManaAmount.GetSymbolsFromColor(_colors));
    }

    public static bool operator ==(ManaUnit left, ManaUnit right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(ManaUnit left, ManaUnit right)
    {
      return !Equals(left, right);
    }

    public IManaAmount ToAmount()
    {
      return new PrimitiveManaAmount(this);
    }
  }
}