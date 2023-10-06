using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Editor.Engine;

namespace Game_Tools_Week4_Editor.Editor
{
    public class GameEditor : Game
    {
        internal Project Project { get; set; }
        
        private GraphicsDeviceManager m_graphics;
        private SpriteBatch _spriteBatch;
        private FormEditor  m_parent;
        /*private Camera m_camera;
          private Models m_teapot;
          private Effect m_myShader;
          private Texture m_metaltexture;*/
        // We have a level to compress all the above data

        public GameEditor()
        {
            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public GameEditor(FormEditor _parent) : this()
        {
            m_parent = _parent;
            Form gameForm = Control.FromHandle(Window.Handle) as Form;
            gameForm.TopLevel = false;
            gameForm.Dock = DockStyle.Fill;
            gameForm.FormBorderStyle = FormBorderStyle.None;    
            m_parent.splitContainer.Panel1.Controls.Add(gameForm);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //m_camera = new Camera(new Vector3(0, 1, 1), m_graphics.GraphicsDevice.Viewport.AspectRatio);
            RasterizerState state = new RasterizerState();
            state.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = state;
            base.Initialize();
        }

        protected override void LoadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (Project != null) Project.Render(gameTime);
            base.Draw(gameTime);
        }

        public void AdjustAspectRatio()
        {
            if (Project == null)return;
            Camera c = Project.CurrentLevel.GetCamera();
            c.Update(c.Position, m_graphics.GraphicsDevice.Viewport.AspectRatio);
        }
    }
}