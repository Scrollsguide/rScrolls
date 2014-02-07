using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;

namespace SGAWrapper
{
  public class ScrollsGuideApi
  {

    public enum ImageSize
    {
      Small, Large
    }

    private const string BaseApiUrl = "http://a.scrollsguide.com/";
    private static readonly WebClient WebClient = new WebClient();

    private ScrollsGuideApi() {}

    public static int GetPlayersOnline()
    {
      dynamic onlinePlayers = GetJson("online");
      return (int) onlinePlayers.data.online;
    }

    public static GameStatistics GetGameStatistics()
    {
      dynamic stats = GetJson("statistics");
      return new GameStatistics(stats);
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

    public static Image GetScrollImage(string name, ImageSize size)
    {
      byte[] data = WebClient.DownloadData(BaseApiUrl + "image/screen?name=" + name + "&size=" + (size == ImageSize.Small ? "small" : "large"));
      using (MemoryStream stream = new MemoryStream(data))
      {
         return Image.FromStream(stream, false, true);
      }
    }

    public static PlayerStatistics GetPlayerStatistics(string name)
    {
      dynamic player = GetJson("player?name=" + name + "&fields=all");
      return new PlayerStatistics(player);
    }

    #region Private helpers methods

    private static dynamic GetJson(string url)
    {
      string json = WebClient.DownloadString(BaseApiUrl + url);
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
