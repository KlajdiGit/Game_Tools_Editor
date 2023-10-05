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

        private static GameTime _gameTime;

        public GameTime GetGameTime()
        {
            return _gameTime;
        }

        public static void SetGameTime(GameTime gameTime)
        {
            _gameTime = gameTime; 
        }

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
            else if(this.Mesh.Tag == "World")
            {
                Random random = new Random();

                m_rotation.Y += _vec.Y;

                //float speed = (float)random.NextDouble() * 0.01f + 0.01f;
                float speed = 0.1f;
                Vector3 origin = Vector3.Zero; // change this if you want your circle's origin elsewhere
                float angle = (float)_gameTime.TotalGameTime.TotalSeconds;
                float radius = Vector3.Distance(Position, origin);


                m_position.X = (float)(Math.Cos(angle * speed) * radius + origin.X);
                m_position.Y = (float)(Math.Sin(angle * speed) * radius + origin.Y);
                m_position.Z = 0.0f; // change this if you want your object to move in 3D space

                Shader.Parameters["World"].SetValue(GetTransform());
                Shader.Parameters["WorldViewProjection"].SetValue(GetTransform() * _view * _projection);
                Shader.Parameters["Texture"].SetValue(Texture);

                foreach (ModelMesh mesh in Mesh.Meshes)
                {
                    mesh.Draw();
                }
            }
            else if(this.Mesh.Tag == "Moon")
            { /*
                    Models world = Level.worlds[0];

                    // Get the parent world's position
                    Vector3 parentPosition = world.Position; // Assuming you have a reference to the parent world object
                                               // Set the moon's position to be 20 units away from the parent in the X direction
                    m_position += parentPosition;
                    // Rotate the moon around its own axis
                    m_rotation.Y += _vec.Y;
               

               
                // Set the shader parameters
                Shader.Parameters["World"].SetValue(GetTransform());
                Shader.Parameters["WorldViewProjection"].SetValue(GetTransform() * _view * _projection);
                Shader.Parameters["Texture"].SetValue(Texture);
                // Draw the moon
                foreach (ModelMesh mesh in Mesh.Meshes)
                {
                    mesh.Draw();
                }*/
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


/* Vector3 distance = m_position - Vector3.Zero;
                distance.Normalize();
                float dot = Vector3.Dot(distance, Vector3.Right); // Calculate the dot product

                float cosine = dot;
                float sine = (float)Math.Sqrt(1.0f - dot * dot); //1.0f - dot * dot;

                if(cosine > 0 && sine >0 )
                {
                    m_position.Y += 0.5f;
                    m_position.X -= 0.5f;
                }

                if (cosine < 0 && sine > 0)
                {
                    m_position.Y -= 0.5f;
                    m_position.X -= 0.5f;
                }

                if (cosine < 0 && sine < 0)
                {
                    m_position.Y -= 0.5f;
                    m_position.X += 0.5f;
                }

                if (cosine > 0 && sine < 0)
                {
                    m_position.Y += 0.5f;
                    m_position.X += 0.5f;
                }*/