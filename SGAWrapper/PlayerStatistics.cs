using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGAWrapper
{
  public class PlayerStatistics
  {

    internal PlayerStatistics(dynamic dyn)
    {
      Name = dyn.data.name;
      Rating = dyn.data.rating;
      GamesPlayed = dyn.data.played;
      GamesWon = dyn.data.won;
      GamesSurrendered = dyn.data.surrendered;
      Gold = dyn.data.gold;
      ScrollsOwned = dyn.data.scrolls;
    }

    public string Name { get; private set; }
    public int Rating { get; private set; }
    public int Rank { get; private set; }
    public int GamesPlayed { get; private set; }
    public int GamesWon { get; private set; }
    public int GamesSurrendered { get; private set; }
    public int Gold { get; private set; }
    public int ScrollsOwned { get; private set; }

    //TODO - Map Achievements

  }
}
