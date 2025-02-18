using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;

namespace EdgeDetectionApp
{
    public partial class Form1 : Form
    {
        // Import Sobel's function from AsmDLL
        // TO-DO: Shorten the path to the file so that anyone can execute it, not just on your computer
        [DllImport("C:\\Users\\Admin\\source\\repos\\edge-detection-app\\EdgeDetectionApp\\x64\\Debug\\AsmDLL.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int MyProc1(int x, int y);

        // Import Sobel's function from CppDLL
        [DllImport("CppDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SobelFilter32bpp(IntPtr data, int width, int height, int stride);

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Function responsible for the event when “Upload” is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Filter that allows you to select the type of file you are uploading
                openFileDialog.Filter = "Bitmap Files|*.bmp";
                openFileDialog.Title = "Select a Bitmap Image";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    // Loading a bitmap and setting it up in pictureBox1
                    pictureBox1.Image = Image.FromFile(filePath);
                }
            }
        }

        /// <summary>
        /// Function responsible for the event when “Save” is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check if there is any processed image in pictureBox2
            if (pictureBox2.Image == null)
            {
                MessageBox.Show("There is no processed image to save. Please process an image first.");
                return;
            }

            // We create a save dialog box
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Bitmap Files|*.bmp";
                saveFileDialog.Title = "Save a Bitmap Image";

                // If the user selected a path and pressed OK
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // We save the image from pictureBox2 in BMP format
                    pictureBox2.Image.Save(saveFileDialog.FileName, ImageFormat.Bmp);
                }
            }
        }


        /// <summary>
        /// Function responsible for the event when “Start” is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, EventArgs e)
        {
            string? selectedOption = comboBox1.SelectedItem?.ToString();

            // Depending on the option selected in comboBox1, logic from the appropriate library is selected
            if (selectedOption == "C++")
            {
                if (pictureBox1.Image == null)
                {
                    MessageBox.Show("Please upload an image first.");
                    return;
                }

                // Make a copy of the image from pictureBox1 so you can modify it
                Bitmap bmp = new Bitmap(pictureBox1.Image);
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

                // Bitmap locking for memory operations
                BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);

                // Calling a function from C++ (SobelFilter32bpp) on locked data
                SobelFilter32bpp(bmpData.Scan0, bmp.Width, bmp.Height, bmpData.Stride);

                // Unlocking the bitmap after the modification is complete
                bmp.UnlockBits(bmpData);

                // Displaying the processed image in pictureBox2
                pictureBox2.Image = bmp;
                pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;

            }
            else if (selectedOption == "Assembly")
            {
                // ASM DLL
                int z = MyProc1(3, 4);
                MessageBox.Show("" + z);
            }
            else
            {
                MessageBox.Show("Please choose which library you prefer.");
            }
        }

        private void ProgressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
