namespace SGAWrapper
{
  public class ScrollsStatistics
  {

    internal ScrollsStatistics(dynamic stats)
    {
      OnlineToday = (int) stats.data.onlinetoday;
      TotalScrolls = (int) stats.data.totalscrolls;
      ScrollsSold = (int)stats.data.scrollssold;
      GoldEarned = (int)stats.data.goldearned;
      PlayersOnline = (int)stats.data.online;
      GamesPlayed = (int)stats.data.gamesplayed;
      TotalUsers = (int)stats.data.totalusers;
    }

    public int OnlineToday { get; private set; }
    public int TotalScrolls { get; private set; }
    public int ScrollsSold { get; private set; }
    public int GoldEarned { get; private set; }
    public int PlayersOnline { get; private set; }
    public int GamesPlayed { get; private set; }
    public int TotalUsers { get; private set; }

  }
}
