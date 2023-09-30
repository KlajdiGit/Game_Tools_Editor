using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Editor.Engine.Interfaces;
using System.IO;
using Game_Tools_Week4_Editor;

namespace Game_Tools_Week4_Editor
{
    internal class Level : ISerializable
    {
        // Accessors
        public Camera GetCamera() { return m_camera; }

        //Members
        private List<Models> m_models = new();
        private Camera m_camera = new(new Vector3(0, 0, 300), 16 / 9);
        private static float RotationSun = 0.005f;

        public Level()
        {
            
        }

        public void LoadSun(ContentManager _content)
        {
            foreach(Models model in m_models) 
            {
                if(model.Mesh.Tag == "Sun")
                {
                    return;
                }
            }
           
            Models sunModel = new(_content, "Sun", "SunDiffuse", "MyShader", Vector3.Zero, 2.0f);
            sunModel.SetShader(_content.Load<Effect>("MyShader"));
            AddModel(sunModel);
        }

        public void LoadContent(ContentManager _content)
        {
            
            Models teapot = new(_content, "Moon" , "MoonDiffuse", "MyShader", Vector3.Zero, 1.0f);
            teapot.SetShader(_content.Load<Effect>("MyShader"));
            AddModel(teapot);
        }

        public void AddModel(Models _model)
        {
            m_models.Add(_model);
        }

        public void Render()
        {
            foreach(Models m in m_models)
            {
                if(m.Mesh.Tag == "Sun")
                m.Render(m_camera.View, m_camera.Projection, RotationSun);
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
