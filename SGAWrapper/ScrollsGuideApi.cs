using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;

namespace SGAWrapper
{
  public class ScrollsGuideApi
  {

    private const string BaseApiUrl = "http://a.scrollsguide.com/";

    private ScrollsGuideApi() {}

    public static int GetPlayersOnline()
    {
      dynamic onlinePlayers = GetJson("online");
      return (int) onlinePlayers.data.online;
    }

    public static ScrollsStatistics GetScrollsStatistics()
    {
      dynamic stats = GetJson("statistics");
      return new ScrollsStatistics(stats);
    }

    public static Scroll GetScroll(string name)
    {
      dynamic scroll = GetJson("scrolls?name=" + name);
      return new Scroll(scroll.data[0]);
    }

    public static Scroll[] GetScrolls(params int[] ids)
    {
      string idRequest = ids.Aggregate("scrolls?id=", (current, id) => current + (id + ","));
      dynamic scrolls = GetJson(idRequest.Remove(idRequest.Length - 1));
      List<Scroll> scrollsList = new List<Scroll>();
      foreach (dynamic scroll in scrolls.data)
      {
        scrollsList.Add(new Scroll(scroll));
      }
      return scrollsList.ToArray();
    }

    public static Scroll[] GetAllScrolls()
    {
      dynamic scrolls = GetJson("scrolls");
      List<Scroll> scrollsList = new List<Scroll>();
      foreach (dynamic scroll in scrolls.data)
      {
        scrollsList.Add(new Scroll(scroll));
      }
      return scrollsList.ToArray();
    }



    #region Private helpers methods

    private static dynamic GetJson(string url)
    {
      WebClient webClient = new WebClient();
      string json = webClient.DownloadString(HttpUtility.HtmlEncode(BaseApiUrl + url));
      dynamic container =  JsonConvert.DeserializeObject(json);
      if (container.msg != "success")
      {
        throw new ScrollsGuideException(container.msg);
      }
      return container;
    }

    #endregion

  }
}
