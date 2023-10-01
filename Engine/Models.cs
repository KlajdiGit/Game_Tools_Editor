using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Editor.Engine.Interfaces;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Game_Tools_Week4_Editor;
using Editor.Engine;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Reflection.Metadata;

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
                   Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) *
                   Matrix.CreateTranslation(Position);
        }

        public void Render(Matrix _view, Matrix _projection,Vector3 _vec)
        {
            if (this.Mesh.Tag == "Sun")
            {
                m_rotation.Y += _vec.Y;

                Shader.Parameters["World"].SetValue(GetTransform());
                Shader.Parameters["WorldViewProjection"].SetValue(GetTransform() * _view * _projection);
                Shader.Parameters["Texture"].SetValue(Texture);

                foreach (ModelMesh mesh in Mesh.Meshes)
                {
                    mesh.Draw();
                }
            }
            else
            {
                m_rotation.Y += _vec.Y;

                Vector3 originalDistance = m_position - Vector3.Zero;
                originalDistance.Normalize();
                float dot = Vector3.Dot(originalDistance, Position); // Calculate the dot product

                float cosine = dot;
                float sine = 1.0f - dot * dot;

                if(cosine > 0 && sine >0 )
                {
                    m_position.Y += 0.5f;
                    m_position.X -= 0.5f;
                }

                else if (cosine < 0 && sine > 0)
                {
                    m_position.Y -= 0.5f;
                    m_position.X -= 0.5f;
                }

                else if (cosine < 0 && sine < 0)
                {
                    m_position.Y -= 0.5f;
                    m_position.X += 0.5f;
                }
                else if (cosine > 0 && sine < 0)
                {
                    m_position.Y += 0.5f;
                    m_position.X += 0.5f;
                }

                Shader.Parameters["World"].SetValue(GetTransform());
                Shader.Parameters["WorldViewProjection"].SetValue(GetTransform() * _view * _projection);
                Shader.Parameters["Texture"].SetValue(Texture);

                foreach (ModelMesh mesh in Mesh.Meshes)
                {
                    mesh.Draw();
                }
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
