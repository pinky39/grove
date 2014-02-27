namespace Grove.Decisions
{
  using System;

  [Serializable]
  public class BooleanResult
  {
    public BooleanResult(bool value)
    {
      IsTrue = value;
    }

    public bool IsTrue { get; private set; }

    public static implicit operator BooleanResult(bool value)
    {
      return new BooleanResult(value);
    }
  }
}