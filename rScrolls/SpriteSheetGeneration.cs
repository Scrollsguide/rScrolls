using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedditSharp;

namespace rScrolls
{
  public class SpriteSheetGeneration
  {
    private const string BaseApi = "http://a.scrollsguide.com/";

    private static WebClient webClient = new WebClient();

    public static void Main(string[] args)
    {

      Console.WriteLine("Reddit Username:");
      string username = Console.ReadLine();
      Console.WriteLine("Reddit Password:");
      string password = Console.ReadLine();

      Console.WriteLine("Gettings list of scrolls.");
      List<ScrollWrapper> scrolls = GetScrolls();
      Console.WriteLine(scrolls.Count + " scrolls found.\n");

      Console.WriteLine("Downloading scroll images.");
      scrolls = DownloadImages(scrolls);
      Console.WriteLine("Images downloaded.\n");

      Console.WriteLine("Saving SpriteSheets.");
      List<string> spriteSheets = SaveSpriteSheets(scrolls);
      Console.WriteLine("SpriteSheets saved.\n");

      Console.WriteLine("Generating CSS.");
      string css = GenerateCSS(scrolls);
      File.WriteAllText("rSrcolls.css", css);
      Console.WriteLine("CSS generated and saved!");

      if (username.Length > 0 && password.Length > 0)
      {
        Reddit reddit = new Reddit();
        AuthenticatedUser me = reddit.LogIn(username, password);
        Subreddit scrollsSubReddit = reddit.GetSubreddit("/r/Scrolls");
        SubredditStyle style = scrollsSubReddit.GetStylesheet();
        foreach (string spriteSheet in spriteSheets)
        {
          byte[] data = File.ReadAllBytes(spriteSheet + ".jpg");
          style.UploadImage(spriteSheet, ImageType.JPEG, data);
        }
        string newCss = Regex.Split(style.CSS, "/**botcss**/")[0] + css;
        style.CSS = newCss;
        style.UpdateCss();
      }
    }

    private static List<ScrollWrapper> GetScrolls()
    {
      List<ScrollWrapper> scrolls = new List<ScrollWrapper>();

      string scrollsJson = webClient.DownloadString(BaseApi + "scrolls");
      JContainer container = JsonConvert.DeserializeObject(scrollsJson) as JContainer;
      if (container != null && ((string)container["msg"]) == "success")
      {
        foreach (JToken token in container["data"])
        {
          string imgUrl = string.Format("{0}image/screen?name={1}&size=small", BaseApi, HttpUtility.HtmlEncode(token["name"]));
          scrolls.Add(new ScrollWrapper()
          {
            Id = ((string)token["id"]),
            Name = ((string)token["name"]),
            ImageURL = imgUrl
          });
        }
      }
      return scrolls;
    }

    private static List<ScrollWrapper> DownloadImages(List<ScrollWrapper> scrolls)
    {
      foreach (ScrollWrapper wrapper in scrolls)
      {
        byte[] data = webClient.DownloadData(wrapper.ImageURL);
        using (MemoryStream stream = new MemoryStream(data))
        {
            wrapper.Image = Image.FromStream(stream, false, true);
        }
        Thread.Sleep(200); // Cheeky sleep to not spam SG
      }
      return scrolls;
    }

    private static List<string> SaveSpriteSheets(List<ScrollWrapper> scrolls)
    {
      // Fuck me this feels a hacky way to do it!
      HashSet<string> spriteSheetNames = new HashSet<string>();
      int spriteSheetCount = 0;
      int spriteCount = 0;
      int locationX = 0, locationY = 0;
      int spriteWidth = scrolls[0].Image.Width;
      int spriteHeight = scrolls[0].Image.Height;
      Bitmap spriteSheet = new Bitmap(spriteWidth * 4, spriteHeight * 4);
      Graphics g = Graphics.FromImage(spriteSheet);
      g.Clear(Color.Transparent);
      foreach (ScrollWrapper scroll in scrolls)
      {
        g.DrawImage(scroll.Image, locationX, locationY);
        scroll.Location = new Point(locationX, locationY);
        locationX += spriteWidth;
        spriteCount++;
        if ((spriteCount % 4) == 0)
        {
          locationY += spriteHeight;
          locationX = 0;
        }
        scroll.SpriteSheetName = "spritesheet-" + spriteSheetCount;
        spriteSheetNames.Add(scroll.SpriteSheetName);
        if (spriteCount == 16)
        {
          g.Dispose();
          spriteSheet.Save("spritesheet-" + spriteSheetCount++ + ".jpg");
          spriteSheet.Dispose();
          spriteSheet = new Bitmap(spriteWidth * 4, spriteHeight * 4);
          g = Graphics.FromImage(spriteSheet);
          locationY = 0;
          locationX = 0;
          spriteCount = 0;
        }
      }
      g.Dispose();
      spriteSheet.Save("spritesheet-" + spriteSheetCount + ".jpg");
      spriteSheet.Dispose();

      return new List<string>(spriteSheetNames);
    }

    private static string GenerateCSS(List<ScrollWrapper> scrolls)
    {
      string statichover = "font-size: 0em; height: 375px; width: 210px; z-index: 6;";
      string staticafter = " margin-left: 1px;  font-size: 0.6em; color: rgb(255,137,0);";
      string staticallrules = "{display: inline-block; cursor:default; clear: both; padding-top:5px; margin-right: 2px;}";
      string css = "";
      string allCss = "\n";
      foreach (ScrollWrapper scroll in scrolls)
      {
        string name = scroll.Name.ToLower().Replace(" ", "");
        allCss += ".content a[href=\"#" + name + "\"],";
        css += (".content a[href=\"#" + name + "\"]:hover {" + statichover + " background-image: url(%%" +
                scroll.SpriteSheetName + "%%);  background-position: -" + scroll.Location.X + "px -" + scroll.Location.Y +
                "px; }\n");
        css += ".content a[href=\"#" + name + "\"]::after {" + staticafter + " content: \"[" + scroll.Name + "]\";}\n";
      }
      allCss = allCss.Remove(allCss.Length - 2, 2) + staticallrules;
      css += allCss;
      return css;
    }

  }
}
