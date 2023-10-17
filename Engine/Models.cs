using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Editor.Engine.Interfaces;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Game_Tools_Week4_Editor;
using Editor.Engine;
using System.ComponentModel;
using System.Collections.Generic;

namespace Game_Tools_Week4_Editor
{

    public enum TextureVal
    {
        Metal,
        Grass,
        HeightMap

    }

    public class TextureValConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new List <string>{ TextureVal.Metal.ToString(), TextureVal.Grass.ToString(), TextureVal.HeightMap.ToString() });

        }
    }

    class Models : ISerializable, INotifyPropertyChanged
    {

        [Browsable(false)]
        public Model Mesh { get; set; }

        [Browsable(false)]
        public Effect Shader { get; set; }

        [Browsable(false)]
        public Texture Texture { get; set; }

        private TextureVal _textureVal;

        [Category("Appearance")]
        [TypeConverter(typeof(TextureValConverter))]
        public string DiffuseTexture
        {
            get {
                  if(_textureVal == TextureVal.Metal)
                  {
                    return "Metal";
                  }
                  else if(_textureVal == TextureVal.Grass)
                  {
                    return "Grass";
                  }
                  else if(_textureVal == TextureVal.HeightMap)
                  {
                    return "HeightMap";
                  }
                return "Metal";

            }
            set
            {
                if(value == TextureVal.Metal.ToString()) 
                {
                    _textureVal = TextureVal.Metal;

                }
                else if (value == TextureVal.Grass.ToString())
                {
                    _textureVal = TextureVal.Grass;

                }
                else 
                {
                    _textureVal = TextureVal.HeightMap;

                }
                OnPropertyChanged("DiffuseTexture");
            }
        }



        /*
        public class DiffuseTextureConverter : StringConverter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new List<string> { "Metal", "Grass", "HeightMap" });
            }
        }

        [Category("Appearance")]
        [TypeConverter(typeof(DiffuseTextureConverter))]
        public string DiffuseTexture { get; set; }
        */

        [Category("State")]
        public bool Selected {  get; set; } = false;



        [Category("Transformation")]
        public Vector3 Position { get => m_position; set { m_position = value; } }

        [Category("Transformation")]

        public Vector3 Rotation { get => m_rotation; set { m_rotation = value; } }

        [Category("Transformation")]
        public float Scale { get; set; }

        //Members
        private Vector3 m_position;
        private Vector3 m_rotation;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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

        public void Translate(Vector3 _translate, Camera _camera)
        {
            float distance = Vector3.Distance(_camera.Target, _camera.Position);
            Vector3 forward = _camera.Target - _camera.Position;
            forward.Normalize();
            Vector3 left = Vector3.Cross(forward, Vector3.Up);
            left.Normalize();
            Vector3 up = Vector3.Cross(left, forward);
            up.Normalize();
            Position += left * _translate.X * distance;
            Position += up * _translate.Y * distance;
            Position += forward * _translate.Z * 100f;
        }

        public void Rotate(Vector3 _rotate)
        {
            Rotation += _rotate;
        }

        public Matrix GetTransform()
        {
            return Matrix.CreateScale(Scale) *
                   Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) *
                   Matrix.CreateTranslation(Position);
        }

        public void Render(Matrix _view, Matrix _projection)
        {
            // m_rotation.X += 0.001f;
            // m_rotation.Y += 0.005f;

            Shader.Parameters["World"].SetValue(GetTransform());
            Shader.Parameters["WorldViewProjection"].SetValue(GetTransform() * _view * _projection);
            Shader.Parameters["Texture"].SetValue(Texture);
            Shader.Parameters["Tint"].SetValue(Selected);

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
