namespace Grove.Core.Cards.Modifiers
{
  using System.Diagnostics;

  public class Value
  {
    private ValueType _type;
    private int _value;

    private Value() {}

    public static Value PlusX
    {
      get
      {
        return new Value
          {
            _type = ValueType.PlusX
          };
      }
    }

    public static Value MinusX
    {
      get
      {
        return new Value
          {
            _type = ValueType.MinusX
          };
      }
    }

    public static implicit operator Value(int value)
    {
      return new Value
        {
          _type = ValueType.Constant,
          _value = value
        };
    }

    public int GetValue(int? x)
    {
      if (_type == ValueType.Constant)
        return _value;

      Debug.Assert(x.HasValue);

      return _type == ValueType.MinusX ? -x.Value : x.Value;
    }

    private enum ValueType
    {
      Constant,
      MinusX,
      PlusX
    }
  }
}