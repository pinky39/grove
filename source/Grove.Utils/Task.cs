namespace Grove.Utils
{
  public abstract class Task
  {
    public abstract bool Execute(Arguments arguments);
    public abstract void Usage();
  }
}