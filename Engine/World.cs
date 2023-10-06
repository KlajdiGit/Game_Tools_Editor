using Editor.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Game_Tools_Week4_Editor.Engine
{
    class World : Models
    {
        public List<Models> Moons { get; set; }
        public Models Planet {  get; set; }


        public void AddPlanet(Models model) 
        {
            Planet = model;
        }

        public void AddMoons(Models moons)
        {
            Moons.Add(moons);
        }


        


    }
}
