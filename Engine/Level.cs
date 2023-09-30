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

namespace Game_Tools_Week4_Editor
{
    internal class Level : ISerializable
    {
        // Accessors
        public Camera GetCamera() { return m_camera; }

        //Members
        private List<Models> m_models = new();
        private Camera m_camera = new(new Vector3(0, 0, 300), 16 / 9);

        public Level()
        {
            
        }

        public void LoadContent(ContentManager _content, string _id)
        {
            /*Models teapot = new(_content, "Moon" , "MoonDiffuse", "MyShader", Vector3.Zero, 1.0f);
            teapot.SetShader(_content.Load<Effect>("MyShader"));
            AddModel(teapot);*/

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
            float x = (float) rand.NextDouble() * 300 - 150;
            float y = (float)rand.NextDouble() * 180 - 90;


            Vector3 pos = new Vector3(100.0f, 100.0f, 100.0f);
            Models worldModel = new(_content, "World", "WorldDiffuse", "MyShader", new Vector3(x, y, 0.0f), 0.75f);
            worldModel.SetShader(_content.Load<Effect>("MyShader"));
            AddModel(worldModel);
        }

        public void LoadMoon(ContentManager _content)
        {
            Random rand = new Random();

            for( int i = m_models.Count - 1; i >= 0; i-- ) 
            {
                var model = m_models[i];
                if (model.Mesh.Tag == "World")
                {
                    float scale = (float)(rand.NextDouble() * 0.2d + 0.2d);
                    //new Vector3(model.Position.X, 0.0f, 0.0f);
                    Models moonModel = new(_content, "Moon", "MoonDiffuse", "MyShader", new Vector3(model.Position.X + 20.0f, model.Position.Y, model.Position.Z), scale);
                    moonModel.SetShader(_content.Load<Effect>("MyShader"));
                    AddModel(moonModel);
                }
            }
            
        }

        public void AddModel(Models _model)
        {
            m_models.Add(_model);
        }

       
        public void Render()
        {
            Random rand = new Random();
            float rotation;
            foreach(Models m in m_models)
            {
                if(m.Mesh.Tag == "Sun")
                {
                    m.Render(m_camera.View, m_camera.Projection, new Vector3(0.0f, 0.005f, 0.0f));
                }
                else if(m.Mesh.Tag == "World")
                {
                    //rotate around its own centre, y - axis at a speed of 0.02 to 0.03 units per frame; and
                    rotation = (float)(rand.NextDouble() * 0.01d + 0.02d);
                    m.Render(m_camera.View, m_camera.Projection, new Vector3(0.0f, rotation, 0.0f));
                }
                else if (m.Mesh.Tag == "Moon")
                {
                    // rotate around its own centre, y-axis at a speed of 0.005 to 0.01 units per frame; and
                    rotation = (float)(rand.NextDouble() * 0.005d + 0.005d);
                    m.Render(m_camera.View, m_camera.Projection, new Vector3(0.0f, rotation, 0.0f));
                }
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
