using IQD_UI_Library;
using System.Windows;
using System.Windows.Controls;

namespace Interface.Pages.UserControles
{
   
    public partial class ctrlAddBrach : UserControl
    {
        clsQuestion? _qustion = null;

        public ctrlAddBrach(clsQuestion qustion)
        {
            InitializeComponent();
            _qustion = qustion;

        }

       

        private void btnAddPointToBranch_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(txtQustionTitle.Text)|| string.IsNullOrEmpty(txtQNum.Text)|| string.IsNullOrEmpty(txtNumberOfAnswers.Text))
            {
                IQD_MessageBox.Show("تحذير", "يجب ملئ المعلومات", MessageBoxType.Warning);
                return;
            }

            _qustion?.AddBranch('A');

            ctrlQustionListView listQustion = new ctrlQustionListView();
            listQustion.QuestionIsSelected += Qustion_QuestionStateChanged;

            MainGrid.Visibility= Visibility.Collapsed;
            SubGrid.Visibility= Visibility.Visible;
            SubGrid.Children.Clear();
            SubGrid.Children.Add(listQustion);



        }

        private void Qustion_QuestionStateChanged(object? sender, ( bool IsChecked, string Qustion) e)
        {
            if (!e.IsChecked && _qustion?.PointList?.Count > 0) 
            {
                foreach (var p in _qustion.PointList)
                {
                    if (p.Text == e.Qustion)
                    {
                        _qustion.PointList.Remove(p);
                        return;
                    }
                }
            
            }
            else
            {
                _qustion?.AddPointToBranch('A', new clsPoint(e.Qustion));

            }

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed; 
        }
    }
}
