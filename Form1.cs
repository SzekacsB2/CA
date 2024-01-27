using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CA
{
    public partial class Form1 : Form
    {
        Pattern pattern;
        int width;
        int height;
        public Form1()
        {
            InitializeComponent();
            Reset();
        }

        private void Reset()
        {
            pattern = new Pattern();
            width = pictureBox.Width;
            height = pictureBox.Height;
            Render();
        }

        private void Reset(string path)
        {
            pattern = new Pattern(path);
            width = pictureBox.Width;
            height = pictureBox.Height;
            Render();
        }

        private void Render()
        {
            using (var bmp = new Bitmap(width, height))
            using (var gfx = Graphics.FromImage(bmp))
            using (var brush = new SolidBrush(Color.LightGreen))
            {
                gfx.Clear(ColorTranslator.FromHtml("#2f3539"));

                var cellSize = new Size(height / 70, width / 70);

                foreach (Choord cell in pattern.AliveCells)
                {
                    var cellLocation = new Point(cell.X, cell.Y);
                    var cellRect = new Rectangle(cellLocation, cellSize);
                    gfx.FillRectangle(brush, cellRect);
                }

                pictureBox.Image?.Dispose();
                pictureBox.Image = (Bitmap)bmp.Clone();
            }
        }
        private void timerTick(object sender, EventArgs e)
        {
            pattern.Update();
            Render();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
