namespace Grove.Infrastructure
{
  using System;

  public static class ColorUtils
  {
    // http://martin.ankerl.com/2009/12/09/how-to-create-random-colors-programmatically/
    public static byte[] HsvToRgb(float h, float s, float v)
    {
      var hI = Convert.ToInt32(h*6);      
      var f = h*6 - hI;      
      var p = v*(1 - s);
      var q = v*(1 - f*s);
      var t = v*(1 - (1 - f)*s);

      var rgb = new[] {0f, 0f, 0f};

      switch (hI)
      {
        case (0):
          {
            rgb = new[] {v, t, p};
            break;
          }
        case (1):
          {
            rgb = new[] { q, t, p };
            break;
          }
        case (2):
          {
            rgb = new[] { p, v, t };
            break;
          }
        case (3):
          {
            rgb = new[] { p, q, v };
            break;
          }

        case (4):
          {
            rgb = new[] { t, p, v };
            break;
          }

        case (5):
          {
            rgb = new[] { v, p, q };
            break;
          }
      }

      return new[]
        {
          (byte)(256 *rgb[0]), 
          (byte)(256 *rgb[1]), 
          (byte)(256 *rgb[2])
        };      
    }
  }
}