using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGAWrapper
{
  public class Scroll
  {

    public class Rule
    {
      
      internal Rule(dynamic dyn)
      {
        Name = dyn.name;
        Description = dyn.description;
      }

      public string Name { get; private set; }
      public string Description { get; private set; }

    }

    public class Ability
    {
      
      internal Ability(dynamic dyn)
      {
        Name = dyn.name;
        Description = dyn.description;
      }

      public string Name { get; private set; }
      public string Description { get; private set; }

    }

    internal Scroll(dynamic dyn)
    {
      Id = dyn.id;
      Name = dyn.name;
      Description = dyn.description;
      Kind = dyn.kind;
      Types = dyn.types;
      GrowthCost = dyn.costgrowth;
      OrderCost = dyn.costorder;
      EnergyCost = dyn.costenergy;
      DecayCost = dyn.costdecay;
      Attack = dyn.ap;
      Countdown = dyn.ac;
      Hitpoints = dyn.hp;
      Flavor = dyn.flavor;
      Rarity = dyn.rarity;
      List<Rule> rules = new List<Rule>();
      if (dyn.GetType().GetProperty("passiverules") != null)
      {
        foreach (dynamic passiverule in dyn.passiverules)
        {
          rules.Add(new Rule(passiverule));
        }
      }
      Rules = rules.ToArray();
      List<Ability> abilities = new List<Ability>();
      if (dyn.GetType().GetProperty("abilities") != null)
      {
        foreach (dynamic ability in dyn.abilities)
        {
          abilities.Add(new Ability(ability));
        }
      }
      Abilities = abilities.ToArray();
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Kind { get; private set; }
    public string Types { get; private set; }
    public int GrowthCost { get; private set; }
    public int OrderCost { get; private set; }
    public int EnergyCost { get; private set; }
    public int DecayCost { get; set; }
    public int Attack { get; private set; }
    public int Countdown { get; private set; }
    public int Hitpoints { get; private set; }
    public string Flavor { get; private set; }
    public int Rarity { get; private set; }
    public Rule[] Rules { get; private set; }
    public Ability[] Abilities { get; private set; }




  }
}
