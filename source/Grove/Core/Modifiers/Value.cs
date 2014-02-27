namespace Grove.Modifiers
{
  using Grove.Effects;
  using Grove.Infrastructure;

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

      Asrt.True(x != null,
        "X was not specified, did you forgot to define the CostRule? Please note that if you use X in timing rules cost rule must be defined before timing rules.");            

      return _type == ValueType.MinusX ? -x.Value : x.Value;
    }

    public static implicit operator DynParam<int>(Value value)
    {
      return new DynParam<int>(
        getter: (e, g) => value.GetValue(e.X),        
        evaluateOnResolve: false);
    }

    private enum ValueType
    {
      Constant,
      MinusX,
      PlusX
    }
  }
}