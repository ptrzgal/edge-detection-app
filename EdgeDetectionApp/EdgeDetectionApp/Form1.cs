namespace EdgeDetectionApp
{
    public partial class Form1 : Form
    {
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
            }
            else if (selectedOption == "Assembly")
            {
                // ASM DLL
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
