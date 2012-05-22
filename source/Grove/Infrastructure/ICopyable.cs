namespace Grove.Infrastructure
{
  public interface ICopyable
  {
    void Copy(object original, CopyService copyService);
  }
}