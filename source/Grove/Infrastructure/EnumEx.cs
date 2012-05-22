namespace Grove.Infrastructure
{
  public static class EnumEx
  {
    // source http://stackoverflow.com/questions/677204/counting-the-number-of-flags-set-on-an-enumeration
    public static int GetSetBitCount(long value)
    {
      var count = 0;      
      while (value != 0)
      {
        value = value & (value - 1);
        count++;
      }      
      return count;
    }
  }
}