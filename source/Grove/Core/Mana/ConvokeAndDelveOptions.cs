namespace Grove
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  public class ConvokeAndDelveOptions
  {
    public bool CanUseConvoke;
    public bool CanUseDelve;
    public List<Card> UiDelveSources;
    public List<Card> UiConvokeSources;

    public static readonly ConvokeAndDelveOptions NoConvokeAndDelve = new ConvokeAndDelveOptions();    
  }
}