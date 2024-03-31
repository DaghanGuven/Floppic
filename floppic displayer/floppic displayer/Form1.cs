using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace floppic_displayer
{
    public partial class Form1 : Form
    {
        string FileContent = "0";
        int currentX, currentY = 0;

        public Form1()
        {
            InitializeComponent();
            HandleCommandLineArguments(Environment.GetCommandLineArgs());
        }

        private void HandleCommandLineArguments(string[] args)
        {
            // Check if command-line arguments are provided
            if (args.Length > 1)
            {
                FileContent = File.ReadAllText(args[1]);
                Text = args[1];
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (FileContent == "0") return;
            List<string> Pixels = new List<string>(FileContent.Split());

            int width = Convert.ToInt32(Pixels[0].Split('X')[0]);
            int height = Convert.ToInt32(Pixels[0].Split('X')[1]);

            pictureBox1.Image = new Bitmap(width, height);
            int currentIndex = 1;

            while (currentIndex < Pixels.Count)
            {
                int color = Convert.ToInt32(Pixels[currentIndex].Split('.')[0]);
                int repeatCount = Convert.ToInt32(Pixels[currentIndex].Split('.')[1]);

                for (int i = 0; i < repeatCount; i++)
                {
                    ((Bitmap)pictureBox1.Image).SetPixel(currentX, currentY, Color.FromArgb(color, color, color));

                    currentX++;

                    if (currentX >= width)
                    {
                        currentX = 0;
                        currentY++;
                    }

                    if (currentY >= height)
                    {
                        break;  // Break if we have reached the end of the image
                    }
                }

                currentIndex++;
            }

            pictureBox1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Convertjpgtofloppic().Show();
            Hide();
        }
    }
}
