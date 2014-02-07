using System.Drawing;

namespace SGAWrapper.Examples
{
  class Program
  {
    static void Main(string[] args)
    {

      int playersOnline = ScrollsGuideApi.GetPlayersOnline();

      GameStatistics stats = ScrollsGuideApi.GetGameStatistics();

      Scroll husk = ScrollsGuideApi.GetScroll("husk");
      Scroll royalInfantryMan = ScrollsGuideApi.GetScroll("Royal Infantryman");
      Scroll gravelockElder = ScrollsGuideApi.GetScroll("gravelock elder");

      Scroll[] scrolls = ScrollsGuideApi.GetScrolls(1, 2, 4, 13);

      Scroll[] all = ScrollsGuideApi.GetAllScrolls();

      Image blessingOfHasteImageLarge = ScrollsGuideApi.GetScrollImage("Blessing of Haste",
                                                                       ScrollsGuideApi.ImageSize.Large);
      Image huskImageSmall = ScrollsGuideApi.GetScrollImage("husk", ScrollsGuideApi.ImageSize.Small);

      PlayerStatistics player = ScrollsGuideApi.GetPlayerStatistics("MattyCov");

    }
  }
}
