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
        [DllImport("CppDLL.dll")]
        public static extern IntPtr Create(int x);

        [DllImport("CppDLL.dll")]
        public static extern int AttemptAdd(IntPtr a, int y);
        
        IntPtr a = Create(5);


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
        /// Function responsible for the event when “Start” is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, EventArgs e)
        {
            // Get the selected option from the comboBox1 control and convert it to a string. The ? character
            // indicates that the variable may be null (string?) or prevents exceptions by returning null if
            // the object to the left of the ?. operator is null.
            string? selectedOption = comboBox1.SelectedItem?.ToString();


            // Depending on the option selected in comboBox1, logic from the appropriate library is selected
            if (selectedOption == "C++")
            {
                // CPP DLL
                MessageBox.Show("" + AttemptAdd(a, 10));
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
