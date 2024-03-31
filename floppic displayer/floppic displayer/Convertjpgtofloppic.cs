using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace floppic_displayer
{
    public partial class Convertjpgtofloppic : Form
    {
        public Convertjpgtofloppic()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!(sender is Button)) goto thisthing;
            openFileDialog1.Filter = "JPG files (*.jpg)|*.jpg";
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName == "openFileDialog1") return;

            thisthing:
            textBox1.Text= openFileDialog1.FileName;
            pictureBox1.Image = Image.FromFile(textBox1.Text);
            button2.Enabled = true;
        }

        private void Convertjpgtofloppic_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string imagePath = openFileDialog1.FileName;
            int LastVal=521;
            int Multiply = 1;

            StringBuilder FileContentBuilder = new StringBuilder();

            Bitmap image = new Bitmap(imagePath);

            int width = image.Width;
            int height = image.Height;

            int result;
            int red;
            int green;
            int blue;

            // Loop through each pixel in the image
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                     red = pixelColor.R;
                     green = pixelColor.G;
                     blue = pixelColor.B;
                    result = (red + green + blue) / 3;

                    if (result==LastVal||LastVal==521) {
                        Multiply++;
                        LastVal = result;
                    }
                    else
                    {
                        FileContentBuilder.Append(result+"."+Multiply+' ');
                        LastVal = result;
                        Multiply = 1;
                    }
                }
                progressBar1.Value = Convert.ToInt32(Convert.ToDouble(y) / Convert.ToDouble(height) * 100);
            }

            // Dispose the image object to free resources
            image.Dispose();

            string FileContent = FileContentBuilder.ToString().Remove(FileContentBuilder.ToString().Length-1);

            File.WriteAllText(imagePath.Remove(imagePath.Length - 3) + "floppic", width+"X"+height+" "+FileContent);

        }

        private void Convertjpgtofloppic_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length != 1)
            {
                MessageBox.Show("You can drop only one image.","MULTIPLE FILES DETECTED",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }
            if (!files[0].EndsWith("jpg"))
            {
                MessageBox.Show("You can only choose jpg files.", "NOT A JPG FILE", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            openFileDialog1.FileName = files[0];
            button1_Click("hi",new EventArgs());
        }

        private void Convertjpgtofloppic_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
    }
}
