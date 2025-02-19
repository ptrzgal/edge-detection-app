using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace EdgeDetectionApp
{
    public partial class Form1 : Form
    {
        // Importing Sobel's function from the AsmDLL library
        [DllImport("AsmDLL.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void AsmSobelFunction(IntPtr inputImage, IntPtr outputImage, int width, int height);

        // Importing Sobel's function from the CppDLL library
        [DllImport("CppDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CppSobelFunction(IntPtr inputImage, IntPtr outputImage, int width, int height);

        /// <summary>
        /// Constructor initializing components
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The function responsible for the logic after pressing the "Upload" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Bitmap Files|*.bmp";
                openFileDialog.Title = "Select a Bitmap Image";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    pictureBox1.Image = Image.FromFile(filePath);
                }
            }
        }

        /// <summary>
        /// The function responsible for the logic after pressing the "Save" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image == null)
            {
                MessageBox.Show("There is no processed image to save. Please process an image first.");
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Bitmap Files|*.bmp";
                saveFileDialog.Title = "Save a Bitmap Image";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox2.Image.Save(saveFileDialog.FileName, ImageFormat.Bmp);
                }
            }
        }

        /// <summary>
        /// The function responsible for the logic after pressing the "Start" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, EventArgs e)
        {
            // Check what user has selected in comboBox1 (e.g. "C++" or "Assembly")
            string? selectedOption = comboBox1.SelectedItem?.ToString();

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Please upload an image first.");
                return;
            }

            // Convert the image 32bpp -> 8bpp (gray)
            Bitmap sourceBmp = new Bitmap(pictureBox1.Image);
            int width = sourceBmp.Width;
            int height = sourceBmp.Height;

            byte[] inputGray = new byte[width * height];
            byte[] outputGray = new byte[width * height];

            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = sourceBmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
            try
            {
                IntPtr scan0 = bmpData.Scan0;
                int stride = bmpData.Stride;
                for (int yRow = 0; yRow < height; yRow++)
                {
                    int rowStart = yRow * stride;
                    for (int xCol = 0; xCol < width; xCol++)
                    {
                        int pixelOffset = rowStart + xCol * 4; // 4 bytes/pixel (BGRA)
                        byte b = Marshal.ReadByte(scan0, pixelOffset + 0);
                        byte g = Marshal.ReadByte(scan0, pixelOffset + 1);
                        byte r = Marshal.ReadByte(scan0, pixelOffset + 2);

                        double grayF = 0.299 * r + 0.587 * g + 0.114 * b;
                        if (grayF > 255) grayF = 255;
                        if (grayF < 0) grayF = 0;
                        inputGray[yRow * width + xCol] = (byte)grayF;
                    }
                }
            }
            finally
            {
                sourceBmp.UnlockBits(bmpData);
            }

            // Start counting time
            Stopwatch stopwatch = new Stopwatch();

            if (selectedOption == "C++")
            {
                // Pin the boards
                GCHandle handleIn = GCHandle.Alloc(inputGray, GCHandleType.Pinned);
                GCHandle handleOut = GCHandle.Alloc(outputGray, GCHandleType.Pinned);
                try
                {
                    IntPtr ptrIn = handleIn.AddrOfPinnedObject();
                    IntPtr ptrOut = handleOut.AddrOfPinnedObject();

                    // Calling and timming a Sobel's function from CppDLL
                    stopwatch.Start();
                    CppSobelFunction(ptrIn, ptrOut, width, height);
                    stopwatch.Stop();
                }
                finally
                {
                    handleIn.Free();
                    handleOut.Free();
                }
            }
            else if (selectedOption == "Assembly")
            {
                // Pin the boards
                GCHandle handleIn = GCHandle.Alloc(inputGray, GCHandleType.Pinned);
                GCHandle handleOut = GCHandle.Alloc(outputGray, GCHandleType.Pinned);
                try
                {
                    IntPtr ptrIn = handleIn.AddrOfPinnedObject();
                    IntPtr ptrOut = handleOut.AddrOfPinnedObject();

                    // Calling and timming a Sobel's function from AsmDLL
                    stopwatch.Start();
                    AsmSobelFunction(ptrIn, ptrOut, width, height);
                    stopwatch.Stop();
                }
                finally
                {
                    handleIn.Free();
                    handleOut.Free();
                }
            }
            else
            {
                MessageBox.Show("Please choose which library you prefer.");
                return;
            }

            labelExecutionTime.Text = $"Execution time: {stopwatch.ElapsedMilliseconds} ms";

            // Create a 32bpp output bitmap and copy 8-bit data to it (outputGray)
            Bitmap resultBmp = new Bitmap(width, height, PixelFormat.Format32bppRgb);
            BitmapData resultData = resultBmp.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppRgb
            );
            try
            {
                int strideRes = resultData.Stride;
                IntPtr scanRes = resultData.Scan0;
                for (int yRow = 0; yRow < height; yRow++)
                {
                    int rowStart = yRow * strideRes;
                    for (int xCol = 0; xCol < width; xCol++)
                    {
                        byte gray = outputGray[yRow * width + xCol];
                        // B, G, R set to gray and alpha = 255
                        Marshal.WriteByte(scanRes, rowStart + xCol * 4 + 0, gray); // Blue
                        Marshal.WriteByte(scanRes, rowStart + xCol * 4 + 1, gray); // Green
                        Marshal.WriteByte(scanRes, rowStart + xCol * 4 + 2, gray); // Red
                        Marshal.WriteByte(scanRes, rowStart + xCol * 4 + 3, 255);  // Alpha
                    }
                }
            }
            finally
            {
                resultBmp.UnlockBits(resultData);
            }

            // Display the result in pictureBox2
            pictureBox2.Image = resultBmp;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }
    }
}
