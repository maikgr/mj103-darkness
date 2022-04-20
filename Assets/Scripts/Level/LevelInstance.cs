namespace Mj103Scripts.Level
{
  public class LevelInstance
  {
    public int XSize { get; }
    public int YSize { get; }

    public LevelInstance(int xSize, int ySize)
    {
      this.XSize = xSize;
      this.YSize = ySize;
    }
  }
}