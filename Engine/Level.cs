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
            float x = (float)(rand.NextDouble() * 300.0d - 150.0d);
            float y = (float)(rand.NextDouble() * 180.0d - 90.0d);
            //The speed works but it's too slow
            float speed =(float)(rand.NextDouble() * 0.001d + 0.001d);

            Models worldModel = new(_content, "World", "WorldDiffuse", "MyShader", new Vector3(x, y, 0.0f), 0.75f);
            worldModel.SetSpeed(speed);
            worldModel.SetShader(_content.Load<Effect>("MyShader"));
            worldModel.ElapsedTime = 0.0f; 

            if (_worldNum == 1)
            {
                m_world1.Add(worldModel);
            }

            else if (_worldNum == 2)
            {
                m_world2.Add(worldModel);
            }

            else if (_worldNum == 3)
            {
                m_world3.Add(worldModel);
            }

            else if (_worldNum == 4)
            {
                m_world4.Add(worldModel);  
            }
            else if (_worldNum == 5)
            {
                m_world5.Add(worldModel);
            }

            _worldNum++;
            AddModel(worldModel);
        }


        void CreateMoonModel(List<Models> worldList, ContentManager _content)
        {
            Random rand = new Random();

            if (worldList.Count > 0)
            {
                float scale = (float)(rand.NextDouble() * 0.2d + 0.2d);
                //The speed turned out to work but it's too slow. 
                float speed = (float)(rand.NextDouble() * 0.01d + 0.01d);
                Models moonModel = new(_content, "Moon", "MoonDiffuse", "MyShader", new Vector3(worldList[0].Position.X + 20.0f,
                                       worldList[0].Position.Y, worldList[0].Position.Z), scale);

                moonModel.SetShader(_content.Load<Effect>("MyShader"));
                moonModel.SetSpeed(speed);
                moonModel.ElapsedTime = 0.0f; 

                worldList.Add(moonModel);
                AddModel(moonModel);
            }
        }

        public void LoadMoon(ContentManager _content)
        {

            CreateMoonModel(m_world1, _content);
            CreateMoonModel(m_world2, _content);
            CreateMoonModel(m_world3, _content);
            CreateMoonModel(m_world4, _content);
            CreateMoonModel(m_world5, _content);
        }

        public void AddModel(Models _model)
        {
            m_models.Add(_model);
        }

        void RenderModels(List<Models> worldList, GameTime gameTime)
        {
            GameTime = gameTime;
            Random rand = new Random();
            float rotation;

            if (worldList.Count > 0)
            {
                rotation = (float)(rand.NextDouble() * 0.01d + 0.02d);
                worldList[0].ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                worldList[0].RenderMovingObjects(m_camera.View, m_camera.Projection, new Vector3(0.0f, rotation, 0.0f), Vector3.Zero, worldList[0].ElapsedTime);
            }

            for (int i = 1; i < worldList.Count; i++)
            {
                rotation = (float)(rand.NextDouble() * 0.005d + 0.005d);
                worldList[i].ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                worldList[i].RenderMovingObjects(m_camera.View, m_camera.Projection, new Vector3(0.0f, rotation, 0.0f), worldList[0].Position, worldList[i].ElapsedTime);
            }
        }

        public void Render(GameTime gameTime)
        {

            foreach (Models m in m_models)
            {
                if (m.Mesh.Tag == "Sun")
                {
                    m.Render(m_camera.View, m_camera.Projection, new Vector3(0.0f, 0.005f, 0.0f));
                }
            }
            Random rand = new Random();

            RenderModels(m_world1, gameTime);
            RenderModels(m_world2, gameTime);
            RenderModels(m_world3, gameTime);
            RenderModels(m_world4, gameTime);
            RenderModels(m_world5, gameTime);
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

