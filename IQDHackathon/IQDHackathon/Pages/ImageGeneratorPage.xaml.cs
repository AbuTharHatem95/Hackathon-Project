using System.Windows;
using System.Windows.Controls;
using IQDHackathon;

namespace Interface.Pages
{
    public partial class ImageGeneratorPage : Page
    {
        public ImageGeneratorPage()
        {
            InitializeComponent();
        }

        private void BackToMainWindowButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Window.GetWindow(this).Close();
        }

        private void GenerateImageButton_Click(object sender, RoutedEventArgs e)
        {
            // Logic for generating the image based on user input.
            string description = ImageDescriptionTextBox.Text;

            if (!string.IsNullOrEmpty(description))
            {
                //GeneratedImage.Source = new Uri("https://via.placeholder.com/400x300.png?text=" + description);
            }
            else
            {
                MessageBox.Show("يرجى إدخال وصف للصورة.");
            }
        }
    }
}
