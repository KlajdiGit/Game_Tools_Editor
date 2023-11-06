using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Game_Tools_Week4_Editor.Editor;

namespace Editor.Engine.Interfaces
{
    internal interface ISerializable
    {
        public void Serialize(BinaryWriter _stream);
        public void Deserialize(BinaryReader _stream, GameEditor _game);
    }
}
