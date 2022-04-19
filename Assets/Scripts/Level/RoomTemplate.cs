namespace Mj103Scripts.Level
{

  /**
  * W = Wall
  * E = Empty Ground
  * P = Player Spawwn
  * X = Exit
  **/
  public class RoomTemplate
  {
      public int Type { get; set; }
      public string[][] Templates { get; set; }
  }
}