using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text;
using Game_Tools_Week4_Editor.Editor;

namespace Game_Tools_Week4_Editor /*GUI.Editor*/
{
    public partial class FormEditor : Form
    {
        public GameEditor Game { get; set; }
        private string _id;
        public string Id { get { return _id; } set { _id = value; } }

        public FormEditor()
        {
            InitializeComponent();
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void OurCoolEditor_Load(object sender, EventArgs e)
        {

        }

        private void FormEditor_SizeChanged(object sender, EventArgs e)
        {
            if (Game == null) return;
            Game.AdjustAspectRatio();
        }

        private void splitContainer_Panel1_SizeChanged(object sender, EventArgs e)
        {
            if (Game == null) return;
            Game.AdjustAspectRatio();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        /* private void createToolStripMenuItem_Click(object sender, EventArgs e)
         {
             SaveFileDialog sfd = new();
             if (sfd.ShowDialog() == DialogResult.OK)
             {
                 Game.Project = new(Game.Content, sfd.FileName);
                 Text = "Our Cool Editor - " + Game.Project.Name;
                 Game.AdjustAspectRatio();
             }
             saveToolStripMenuItem_Click(sender, e);
         }*/

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
           /* string fname = Path.Combine(Game.Project.Folder, Game.Project.Name);
            using var stream = File.Open(fname, FileMode.Create);
            using var writer = new BinaryWriter(stream, Encoding.UTF8, false);
            Game.Project.Serialize(writer);*/
        }

        private void addSunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new();
            Id = "Sun";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                //Game.Project = new(Game.Content, sfd.FileName, Id);
                Game.Project.AddLevel(Game.Content, Id);
                Text = "Our Cool Editor - " + Game.Project.Name;
                Game.AdjustAspectRatio();
            }
            saveToolStripMenuItem_Click(sender, e);
        }

        private void addPlanetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Id = "Planet";
            SaveFileDialog sfd = new();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Game.Project.AddLevel(Game.Content, Id);
                Text = "Our Cool Editor - " + Game.Project.Name;
                Game.AdjustAspectRatio();
            }
            saveToolStripMenuItem_Click(sender, e);
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Game.Exit();
        }

        private void loadToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new();
            ofd.Filter = "OCE Files|*.oce";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                using var stream = File.Open(ofd.FileName, FileMode.Open);
                using var reader = new BinaryReader(stream, Encoding.UTF8, false);
                Game.Project = new();
                Game.Project.Deserialize(reader, Game.Content);
                Text = "Our Cool Editor - " + Game.Project.Name;
                Game.AdjustAspectRatio();


            }
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Game.Project = new(Game.Content, sfd.FileName);
                Text = "Our Cool Editor - " + Game.Project.Name;
            }
            saveToolStripMenuItem_Click(sender, e);

        }
    }
}
