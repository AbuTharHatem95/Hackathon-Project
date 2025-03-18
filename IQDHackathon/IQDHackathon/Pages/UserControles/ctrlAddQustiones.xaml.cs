using System.Windows;
using System.Windows.Controls;

namespace Interface.Pages.UserControles
{
    public partial class AddQustiones : UserControl
    {

        public AddQustiones()
        {
            InitializeComponent();
            
        }

        private void btnAddQustion_Click(object sender, RoutedEventArgs e)
        {
            QusetionCreater qusetionCreater = new QusetionCreater();
            AddQustionGrid.Visibility = Visibility.Collapsed;
            QusetionCreaterGrid.Visibility = Visibility.Visible;
            QusetionCreaterFrame.Navigate(qusetionCreater);
        }

    }
}
