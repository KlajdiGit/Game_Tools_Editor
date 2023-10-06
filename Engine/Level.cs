using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Editor.Engine.Interfaces;
using System.IO;
using Game_Tools_Week4_Editor;
using System;
using Microsoft.Xna.Framework.Media;
using System.Reflection;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Diagnostics;
using Game_Tools_Week4_Editor.Engine;

namespace Game_Tools_Week4_Editor
{
    internal class Level : ISerializable
    {
        // Accessors
        public Camera GetCamera() { return m_camera; }

        //Members
        private List<Models> m_models = new();
        
        private List<Models> m_world1 = new();
        private List<Models> m_world2 = new();
        private List<Models> m_world3 = new();
        private List<Models> m_world4 = new();
        private List<Models> m_world5 = new();

        private List<World> m_worlds = new();

        public GameTime GameTime { get; set; }


        private int _worldNum = 1;

        private Camera m_camera = new(new Vector3(0, 0, 300), 16 / 9);


        public Level()
        {
            
        }

        public void LoadContent(ContentManager _content, string _id)
        {
            if(_id.ToLower().CompareTo("sun") == 0)
            {
                LoadSun(_content);
            }
            else if (_id.ToLower().CompareTo("world") == 0)
            {
                LoadWorld(_content);
            }
            else if (_id.ToLower().CompareTo("moon") == 0)
            {
                LoadMoon(_content);
            }
            else
            {
            }
        }

        public void LoadSun(ContentManager _content)
        {
            foreach(var model in m_models) 
            {
                if (model.Mesh.Tag == "Sun")
                {
                    return;
                }
            }

            Models sunModel = new(_content, "Sun", "SunDiffuse", "MyShader", Vector3.Zero, 2.0f);
            sunModel.SetShader(_content.Load<Effect>("MyShader"));
            AddModel(sunModel);
        }

        public void LoadWorld(ContentManager _content)
        {
            World world = new();
            int count = 0;
            foreach (var model in m_models)
            {
                if (model.Mesh.Tag == "World")
                {
                    count++;
                    if(count ==5)
                    {
                        return;
                    }
                }
            }

            Random rand = new Random();
            float x = (float) rand.NextDouble() * 300 - 150;
            float y = (float)rand.NextDouble() * 180 - 90;

            Models worldModel = new(_content, "World", "WorldDiffuse", "MyShader", new Vector3(x, y, 0.0f), 0.75f);
            world.Planet = worldModel;
           // m_worlds.Add(world);
            worldModel.SetShader(_content.Load<Effect>("MyShader"));
            worldModel.ElapsedTime = 0.0f; // Store the elapsed time since the model was created


            if (_worldNum == 1)
            {
                m_world1.Add(worldModel);
                //_worldNum++;
                //Debug.WriteLine($"World1: {m_world1.Count} ");
            }

            else if (_worldNum == 2)
            {
                m_world2.Add(worldModel);
                //Debug.WriteLine($"World2: {m_world2.Count} ");
                //_worldNum++;
            }

            else if (_worldNum == 3)
            {
                m_world3.Add(worldModel);
                //Debug.WriteLine($"World3: {m_world3.Count} ");
                //_worldNum++;
            }

            else if (_worldNum == 4)
            {
                m_world4.Add(worldModel);
                //Debug.WriteLine($"World4: {m_world4.Count} ");
                //_worldNum++;
            }
            else if (_worldNum == 5)
            {
                m_world5.Add(worldModel);
                //Debug.WriteLine($"World5: {m_world5.Count} ");
                //_worldNum++;
            }
            _worldNum++;
            AddModel(worldModel);
        }


        public void LoadMoon(ContentManager _content)
        {
            Random rand = new Random();

            
            if(m_world1.Count >0)
            {
                float scale = (float)(rand.NextDouble() * 0.2d + 0.2d);
                Models moonModel = new(_content, "Moon", "MoonDiffuse", "MyShader", new Vector3(m_world1[0].Position.X + 20.0f, m_world1[0].Position.Y, m_world1[0].Position.Z), scale);
                moonModel.SetShader(_content.Load<Effect>("MyShader"));
                moonModel.ElapsedTime = 0.0f; // Store the elapsed time since the model was created

                m_world1.Add(moonModel);
                AddModel(moonModel);

            }

            if (m_world2.Count > 0)
            {
                float scale = (float)(rand.NextDouble() * 0.2d + 0.2d);
                Models moonModel = new(_content, "Moon", "MoonDiffuse", "MyShader", new Vector3(m_world2[0].Position.X + 20.0f, m_world2[0].Position.Y, m_world2[0].Position.Z), scale);
                moonModel.SetShader(_content.Load<Effect>("MyShader"));
                moonModel.ElapsedTime = 0.0f; // Store the elapsed time since the model was created
                m_world2.Add(moonModel);
                AddModel(moonModel);

            }

            if (m_world3.Count > 0)
            {
                float scale = (float)(rand.NextDouble() * 0.2d + 0.2d);
                Models moonModel = new(_content, "Moon", "MoonDiffuse", "MyShader", new Vector3(m_world3[0].Position.X + 20.0f, m_world3[0].Position.Y, m_world3[0].Position.Z), scale);
                moonModel.SetShader(_content.Load<Effect>("MyShader"));
                moonModel.ElapsedTime = 0.0f; // Store the elapsed time since the model was created

                m_world3.Add(moonModel);
                AddModel(moonModel);

            }

            if (m_world4.Count > 0)
            {
                float scale = (float)(rand.NextDouble() * 0.2d + 0.2d);
                Models moonModel = new(_content, "Moon", "MoonDiffuse", "MyShader", new Vector3(m_world4[0].Position.X + 20.0f, m_world4[0].Position.Y, m_world4[0].Position.Z), scale);
                moonModel.SetShader(_content.Load<Effect>("MyShader"));
                moonModel.ElapsedTime = 0.0f; // Store the elapsed time since the model was created

                m_world4.Add(moonModel);
                AddModel(moonModel);

            }

            if (m_world5.Count > 0)
            {
                float scale = (float)(rand.NextDouble() * 0.2d + 0.2d);
                Models moonModel = new(_content, "Moon", "MoonDiffuse", "MyShader", new Vector3(m_world5[0].Position.X + 20.0f, m_world5[0].Position.Y, m_world5[0].Position.Z), scale);
                moonModel.SetShader(_content.Load<Effect>("MyShader"));
                moonModel.ElapsedTime = 0.0f; // Store the elapsed time since the model was created

                m_world5.Add(moonModel);

                AddModel(moonModel);

            }
            Debug.WriteLine($"World1: {m_world1.Count} ");
            Debug.WriteLine($"World2: {m_world2.Count} ");
            Debug.WriteLine($"World3: {m_world3.Count} ");
            Debug.WriteLine($"World4: {m_world4.Count} ");
            Debug.WriteLine($"World5: {m_world5.Count} ");

        }

        public void AddModel(Models _model)
        {
            m_models.Add(_model);
        }


        public void Render(GameTime gameTime)
        {
            GameTime = gameTime;
            Random rand = new Random();
            float rotation;
    

            foreach (Models m in m_models)
            {
                if (m.Mesh.Tag == "Sun")
                {
                    m.Render(m_camera.View, m_camera.Projection, new Vector3(0.0f, 0.005f, 0.0f));
                }
            }

            if (m_world1.Count > 0)
            {
                rotation = (float)(rand.NextDouble() * 0.01d + 0.02d);
                m_world1[0].ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                m_world1[0].RenderMovingObjects(m_camera.View, m_camera.Projection, new Vector3(0.0f, rotation, 0.0f), Vector3.Zero, m_world1[0].ElapsedTime);
            }


            if (m_world2.Count > 0)
            {
                rotation = (float)(rand.NextDouble() * 0.01d + 0.02d);
                m_world2[0].ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                m_world2[0].RenderMovingObjects(m_camera.View, m_camera.Projection, new Vector3(0.0f, rotation, 0.0f), Vector3.Zero, m_world2[0].ElapsedTime);
            }

            if (m_world3.Count > 0)
            {
                rotation = (float)(rand.NextDouble() * 0.01d + 0.02d);
                m_world3[0].ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                m_world3[0].RenderMovingObjects(m_camera.View, m_camera.Projection, new Vector3(0.0f, rotation, 0.0f), Vector3.Zero, m_world3[0].ElapsedTime);
            }

            if (m_world4.Count > 0)
            {
                rotation = (float)(rand.NextDouble() * 0.01d + 0.02d);
               m_world4[0].ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                m_world4[0].RenderMovingObjects(m_camera.View, m_camera.Projection, new Vector3(0.0f, rotation, 0.0f), Vector3.Zero, m_world4[0].ElapsedTime);
            }

            if (m_world5.Count > 0)
            {
                rotation = (float)(rand.NextDouble() * 0.01d + 0.02d);
               m_world5[0].ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                m_world5[0].RenderMovingObjects(m_camera.View, m_camera.Projection, new Vector3(0.0f, rotation, 0.0f), Vector3.Zero, m_world5[0].ElapsedTime);
            }

            for (int i = 1; i < m_world1.Count; i++)
            {
                // rotate around its own centre, y-axis at a speed of 0.005 to 0.01 units per frame;
                rotation = (float)(rand.NextDouble() * 0.005d + 0.005d);
                // 0.01 to 0.02
                m_world1[i].ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                m_world1[i].RenderMovingObjects(m_camera.View, m_camera.Projection, new Vector3(0.0f, rotation, 0.0f), m_world1[0].Position, m_world1[i].ElapsedTime);
            }

            for (int i = 1; i < m_world2.Count; i++)
            {
                // rotate around its own centre, y-axis at a speed of 0.005 to 0.01 units per frame;
                rotation = (float)(rand.NextDouble() * 0.005d + 0.005d);

                m_world2[i].ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                m_world2[i].RenderMovingObjects(m_camera.View, m_camera.Projection, new Vector3(0.0f, rotation, 0.0f), m_world2[0].Position, m_world2[i].ElapsedTime);
            }

            for (int i = 1; i < m_world3.Count; i++)
            {
                // rotate around its own centre, y-axis at a speed of 0.005 to 0.01 units per frame;
                rotation = (float)(rand.NextDouble() * 0.005d + 0.005d);

                m_world3[i].ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                m_world3[i].RenderMovingObjects(m_camera.View, m_camera.Projection, new Vector3(0.0f, rotation, 0.0f), m_world3[0].Position, m_world3[i].ElapsedTime);
            }

            for (int i = 1; i < m_world4.Count; i++)
            {
                // rotate around its own centre, y-axis at a speed of 0.005 to 0.01 units per frame;
                rotation = (float)(rand.NextDouble() * 0.005d + 0.005d);

                m_world4[i].ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                m_world4[i].RenderMovingObjects(m_camera.View, m_camera.Projection, new Vector3(0.0f, rotation, 0.0f), m_world4[0].Position, m_world4[i].ElapsedTime);
            }

            for (int i = 1; i < m_world5.Count; i++)
            {
                // rotate around its own centre, y-axis at a speed of 0.005 to 0.01 units per frame;
                rotation = (float)(rand.NextDouble() * 0.005d + 0.005d);

                m_world5[i].ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                m_world5[i].RenderMovingObjects(m_camera.View, m_camera.Projection, new Vector3(0.0f, rotation, 0.0f), m_world5[0].Position, m_world5[i].ElapsedTime);
            }
      
        }
 
        

        public void Serialize(BinaryWriter _stream)
        {
            _stream.Write(m_models.Count);
            foreach( var model in m_models)
            {
                model.Serialize(_stream);
            }
            m_camera.Serialize(_stream);
        }

        public void Deserialize(BinaryReader _stream, ContentManager _content)
        {
            int modelCount = _stream.ReadInt32();
            for (int count = 0; count < modelCount; count++)
            {
                Models m = new();
                m.Deserialize(_stream, _content);
                m_models.Add(m);
            }
            m_camera.Deserialize(_stream, _content);
        }
    }
}



/*  else if (m.Mesh.Tag == "World")
    {
        //rotate around its own centre, y - axis at a speed of 0.02 to 0.03 units per frame;
        rotation = (float)(rand.NextDouble() * 0.01d + 0.02d);
        m.ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        m.Render(m_camera.View, m_camera.Projection, new Vector3(0.0f, rotation, 0.0f), Vector3.Zero, m.ElapsedTime);
    }
    else if (m.Mesh.Tag == "Moon")
    {

        if (m_world1.Count > 0)
        {
            for (int i = 1; i < m_world1.Count; i++)
            {
                // rotate around its own centre, y-axis at a speed of 0.005 to 0.01 units per frame;
                rotation = (float)(rand.NextDouble() * 0.005d + 0.005d);
                m_world1[i].Render(m_camera.View, m_camera.Projection, new Vector3(0.0f, rotation, 0.0f), m_world1[0].Position, m.ElapsedTime);
            }
        }


        if (m_world1.Count > 1)
        {
            for (int i = 1; i < m_worlds.Count; i++)
            {
                for (int j = 0; j < m_worlds[i].Moons.Count; j++)
                {
                    rotation = (float)(rand.NextDouble() * 0.005d + 0.005d);
                    //m_worlds[i].Render(m_camera.View, m_camera.Projection, new Vector3(0.0f, rotation, 0.0f), m_world1[0].Position, m.ElapsedTime);
                    m_worlds[i].Moons[i].Render(m_camera.View, m_camera.Projection, new Vector3(0.0f, rotation, 0.0f), m_worlds[i].Position, m.ElapsedTime);
                }
            }
        }

        if (m_world2.Count > 1)
        {
            for (int i = 1; i < m_world2.Count; i++)
            {
                // rotate around its own centre, y-axis at a speed of 0.005 to 0.01 units per frame;
                rotation = (float)(rand.NextDouble() * 0.005d + 0.005d);
                m_world2[i].Render(m_camera.View, m_camera.Projection, new Vector3(0.0f, rotation, 0.0f), m_world2[0].Position, m.ElapsedTime);
            }
        }

        if (m_world3.Count > 1)
        {
            for (int i = 1; i < m_world3.Count; i++)
            {
                // rotate around its own centre, y-axis at a speed of 0.005 to 0.01 units per frame;
                rotation = (float)(rand.NextDouble() * 0.005d + 0.005d);
                m_world3[i].Render(m_camera.View, m_camera.Projection, new Vector3(0.0f, rotation, 0.0f), m_world3[0].Position, m.ElapsedTime);
            }
        }

        if (m_world4.Count > 1)
        {
            for (int i = 1; i < m_world4.Count; i++)
            {
                // rotate around its own centre, y-axis at a speed of 0.005 to 0.01 units per frame;
                rotation = (float)(rand.NextDouble() * 0.005d + 0.005d);
                m_world4[i].Render(m_camera.View, m_camera.Projection, new Vector3(0.0f, rotation, 0.0f), m_world4[0].Position, m.ElapsedTime);
            }
        }

        if (m_world5.Count > 1)
        {
            for (int i = 1; i < m_world5.Count; i++)
            {
                // rotate around its own centre, y-axis at a speed of 0.005 to 0.01 units per frame;
                rotation = (float)(rand.NextDouble() * 0.005d + 0.005d);
                m_world5[i].Render(m_camera.View, m_camera.Projection, new Vector3(0.0f, rotation, 0.0f), m_world5[0].Position, m.ElapsedTime);
            }
        } */


/* // rotate around its own centre, y-axis at a speed of 0.005 to 0.01 units per frame;
 rotation = (float)(rand.NextDouble() * 0.005d + 0.005d);
 m.Render(m_camera.View, m_camera.Projection, new Vector3(0.0f, rotation, 0.0f), m.ElapsedTime);*/




/* for( int i = m_models.Count - 1; i >= 0; i-- ) 
 {
     var model = m_models[i];
     if (model.Mesh.Tag == "World")
     {
         float scale = (float)(rand.NextDouble() * 0.2d + 0.2d);
         //new Vector3(model.Position.X, 0.0f, 0.0f);
         Models moonModel = new(_content, "Moon", "MoonDiffuse", "MyShader", new Vector3(model.Position.X + 20.0f, model.Position.Y, model.Position.Z), scale);
         moonModel.SetShader(_content.Load<Effect>("MyShader"));
         m_world1.Add(moonModel);
         m_world2.Add(moonModel);
         m_world3.Add(moonModel);
         m_world4.Add(moonModel);
         m_world5.Add(moonModel);

         *//*foreach(World w in m_worlds)
         {
             w.AddMoons(moonModel);
         }*//*





         AddModel(moonModel);
     }
 }*/