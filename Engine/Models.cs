﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Editor.Engine.Interfaces;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Game_Tools_Week4_Editor;
using Editor.Engine;

namespace Game_Tools_Week4_Editor
{
    class Models : ISerializable
    {
        // Accessors
        public Model Mesh { get; set; }
        public Effect Shader { get; set; }
        public Vector3 Position { get => m_position; set { m_position = value; } }
        public Vector3 Rotation { get => m_rotation; set { m_rotation = value; } }
        public float Scale { get; set; }


        // Texturing
        public Texture Texture { get; set; }

        //Members
        private Vector3 m_position;
        private Vector3 m_rotation;

        public Models()
        {
        }

        public Models( ContentManager _content,
                       string _model, 
                       string _texture,
                       string _effect,
                       Vector3 _position,
                       float _scale)
        {
            Create(_content, _model, _texture, _effect, _position, _scale);
        }

        public void Create(ContentManager _content,
                       string _model,
                       string _texture,
                       string _effect,
                       Vector3 _position,
                       float _scale)
        {
            Mesh = _content.Load<Model>( _model );
            Mesh.Tag = _model;
            Texture = _content.Load<Texture>( _texture );
            Texture.Tag = _texture;
            Shader = _content.Load<Effect>( _effect );
            Shader.Tag = _effect;
            SetShader(Shader);
            m_position = _position;
            Scale = _scale;
        }

        public void SetShader(Effect _effect)
        {
            Shader = _effect;
            foreach(ModelMesh mesh in Mesh.Meshes)
            {
                foreach(ModelMeshPart meshPart in mesh.MeshParts) 
                {
                    meshPart.Effect = Shader;
                }
            }
        }

        public Matrix GetTransform()
        {
            return Matrix.CreateScale(Scale) *
                   Matrix.CreateRotationY(Rotation.Y) *
                   Matrix.CreateTranslation(Position);
        }

        public void Render(Matrix _view, Matrix _projection, float rotationSpeed)
        {
            m_rotation.X = 0.0f;
            m_rotation.Z = 0.0f;
            m_rotation.Y += rotationSpeed;

            Shader.Parameters["World"].SetValue(GetTransform());
            Shader.Parameters["WorldViewProjection"].SetValue(GetTransform() * _view * _projection);
            Shader.Parameters["Texture"].SetValue(Texture);

            foreach(ModelMesh mesh in Mesh.Meshes)
            {
                mesh.Draw();
            }
        }

        public void Serialize(BinaryWriter _stream)
        {
            _stream.Write(Mesh.Tag.ToString());
            _stream.Write(Texture.Tag.ToString());
            _stream.Write(Shader.Tag.ToString());
            HelperSerialize.Vec3(_stream, Position);
            HelperSerialize.Vec3(_stream, Rotation);
            _stream.Write(Scale);
        }

        public void Deserialize(BinaryReader _stream, ContentManager _content)
        {
            string mesh = _stream.ReadString();
            string texture = _stream.ReadString();
            string shader = _stream.ReadString();
            Position = HelperDeserialize.Vec3(_stream);
            Rotation = HelperDeserialize.Vec3(_stream);
            Scale = _stream.ReadSingle();
            Create(_content, mesh, texture, shader, Position, Scale);
        }
    }
}