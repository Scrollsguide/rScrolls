namespace SGAWrapper.Examples
{
  class Program
  {
    static void Main(string[] args)
    {

      int playersOnline = ScrollsGuideApi.GetPlayersOnline();

      ScrollsStatistics stats = ScrollsGuideApi.GetScrollsStatistics();

      Scroll husk = ScrollsGuideApi.GetScroll("husk");
      Scroll royalInfantryMan = ScrollsGuideApi.GetScroll("Royal Infantryman");
      Scroll gravelockElder = ScrollsGuideApi.GetScroll("gravelock elder");

      Scroll[] scrolls = ScrollsGuideApi.GetScrolls(1, 2, 4, 13);

      Scroll[] all = ScrollsGuideApi.GetAllScrolls();

    }
  }
}
