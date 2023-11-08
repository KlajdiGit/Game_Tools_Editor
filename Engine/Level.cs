﻿using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Editor.Engine.Interfaces;
using System.IO;
using Game_Tools_Week4_Editor;
using System.Windows.Forms;
using Editor.Engine;
using System.Security.Cryptography.Xml;
using Game_Tools_Week4_Editor.Engine.Interfaces;
using Game_Tools_Week4_Editor.Editor;

namespace Game_Tools_Week4_Editor
{
    internal class Level : ISerializable
    {
        // Accessors
        public Camera GetCamera() { return m_camera; }

        //Members
        private List<Models> m_models = new();
        private Camera m_camera = new(new Vector3(0, 400, 500), 16 / 9);
        private Effect m_terrainEffect = null;
        private Terrain m_terrain = null;

        public Level()
        {
            
        }

        public void LoadContent(GraphicsDevice _device, ContentManager _content)
        {
            //m_terrainEffect = _content.Load<Effect>("TerrainEffect");
            //m_terrain = new(_content.Load<Texture2D>("HeightMap"), _content.Load<Texture2D>("Grass"), 200, _device);

        }

        public void AddModel(Models _model)
        {
            m_models.Add(_model);
        }

        public List<ISelectable> GetSelectedModels()
        {
            List<ISelectable> models = new();
            foreach (var model in m_models)
            {
                if(model.Selected) models.Add(model);
            }
            if (m_terrainEffect != null)
            {
                if (m_terrain.Selected) models.Add(m_terrain);
            }
            return models;
        }

        public void Render()
        {
            foreach (Models m in m_models)
            {
                m.Render(m_camera.View, m_camera.Projection);
            }
            if (m_terrain != null)
            { 
                m_terrain.Draw(m_terrainEffect, m_camera.View, m_camera.Projection);
            }
        }

        private void HandleTranslate()
        {
            InputController ic = InputController.Instance;
            Vector3 translate = Vector3.Zero;

            if (ic.isKeyDown(Keys.Left)) translate.X += -10;
            if (ic.isKeyDown(Keys.Right)) translate.X += 10;
            if (ic.isKeyDown(Keys.Menu)) // menu == alt key
            {
                if (ic.isKeyDown(Keys.Up)) translate.Z += 1;
                if (ic.isKeyDown(Keys.Down)) translate.Z += -1;

            }
            else
            {
                if (ic.isKeyDown(Keys.Up)) translate.Y += 10;
                if (ic.isKeyDown(Keys.Down)) translate.Y += -10;
            }
            if (ic.IsButtonDown(MouseButtons.Middle))
            {
                Vector2 dir = ic.MousePosition - ic.LastPosition;
                translate.X = dir.X;
                translate.Y = -dir.Y;
            }
            if (ic.GetWheel() != 0)
            {
                translate.Z = ic.GetWheel() * 2;
            }

            if (translate != Vector3.Zero)
            {
                bool modelTranslated = false;
                foreach(Models model in m_models) 
                {
                    if(model.Selected)
                    {
                        modelTranslated = true;
                        model.Translate(translate / 1000, m_camera);
                    }
                }
                if (!modelTranslated)
                {
                    m_camera.Translate(translate * 0.001f);
                }
            }
        }


        private void HandleRotate(float _delta)
        {
            InputController ic = InputController.Instance;

            if (ic.IsButtonDown(MouseButtons.Right) &&
                 (!ic.isKeyDown(Keys.Menu)))
            {
                Vector2 dir = ic.MousePosition - ic.LastPosition;
                if (dir != Vector2.Zero)
                {
                    Vector3 movement = new Vector3(dir.Y, dir.X, 0) * _delta;
                    bool modelRotated = false;
                    foreach(Models model in m_models) 
                    {
                        if (model.Selected) 
                        {
                            modelRotated = true;
                            model.Rotation += movement;
                        }
                    }
                    if(!modelRotated) 
                    {
                        m_camera.Rotate(movement);
                    }
                }
            }
        }

        private void HandleScale(float _delta)
        {
            InputController ic = InputController.Instance;
            if( (ic.IsButtonDown(MouseButtons.Right)) &&
                 (ic.isKeyDown(Keys.Menu)))
            {
                float l = ic.MousePosition.X - ic.LastPosition.X;
                if(l != 0)
                {
                    l *= _delta;
                    foreach(Models model in m_models)
                    {
                        if(model.Selected)
                        {
                            model.Scale += l;
                        }
                    }
                }
            }
        }
        

        private void HandlePick()
        {
            float? f;
            Matrix transform = Matrix.Identity;

            InputController ic = InputController.Instance;
            if(ic.IsButtonDown(MouseButtons.Left))
            {
                Ray r = HelpMath.GetPickRay(ic.MousePosition, m_camera);
                //Check models
                foreach(Models model in m_models)
                {
                    model.Selected = false;
                    transform = model.GetTransform();
                    foreach(ModelMesh mesh in model.Mesh.Meshes)
                    {
                        BoundingSphere s = mesh.BoundingSphere;
                        s.Transform(ref transform, out s);
                        f = r.Intersects(s);
                        if(f.HasValue)
                        {
                            f = HelpMath.PickTriangle(in mesh, ref r, ref transform);
                            if(f.HasValue)
                            {
                                model.Selected = true;
                            }
                        }
                    }
                }

                //Check terrain
                if (m_terrain != null)
                {
                    transform = Matrix.Identity;
                    f = HelpMath.PickTriangle(in m_terrain, ref r, ref transform);
                    m_terrain.Selected = false;
                    if (f.HasValue)
                    {
                        m_terrain.Selected = true;
                    }
                }
            }
        }

        public void Update(float _delta)
        {
            HandleTranslate();
            HandleRotate(_delta);
            HandleScale(_delta);
            HandlePick();
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

        public void Deserialize(BinaryReader _stream, GameEditor _game)
        {
            int modelCount = _stream.ReadInt32();
            for (int count = 0; count < modelCount; count++)
            {
                Models m = new();
                m.Deserialize(_stream, _game);
                m_models.Add(m);
            }
            m_camera.Deserialize(_stream, _game);
        }

        public override string ToString()
        {
            string s = string.Empty;
            foreach(Models m in m_models) 
            {
                if(m.Selected)
                {
                    s += "\nModel: Pos: " + m.Position.ToString() +
                         "  Rot: " + m.Rotation.ToString();
                }
            }
            return m_camera.ToString() + s;
        }

    }
}
