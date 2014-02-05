using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rScrolls
{
  public class ScrollWrapper
  {

    // Download Stuff
    public string Name { get; set; }
    public string ImageURL { get; set; }
    public string Id { get; set; }

    //SpriteSheet Stuff
    public string SpriteSheetName { get; set; }
    public string SpriteSheetFolder { get; set; }
    public Image Image { get; set; }

    // CSS Stuff
    public Point Location { get; set; }


    public override bool Equals(object obj)
    {
      return obj is ScrollWrapper && obj.GetHashCode() == GetHashCode();
    }

    public override int GetHashCode()
    {
      return int.Parse(Id);
    }

  }
}
