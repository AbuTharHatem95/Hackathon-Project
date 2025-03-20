using IQD_UI_Library;
using System.Windows;
using System.Windows.Controls;

namespace Interface.Pages.UserControles
{
   
    public partial class ctrlAddBrach : UserControl
    {
        clsQuestion? _qustion = null;
        QusetionCreater _qusetionCreater;

        public ctrlAddBrach(clsQuestion qustion,QusetionCreater qustionpage)
        {
            InitializeComponent();
            _qustion = qustion;
            _qusetionCreater = qustionpage;

        }

        private void btnAddPointToBranch_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(txtQustionTitle.Text)|| string.IsNullOrEmpty(txtQNum.Text)|| string.IsNullOrEmpty(txtNumberOfAnswers.Text))
            {
                IQD_MessageBox.Show("تحذير", "يجب ملئ المعلومات", MessageBoxType.Warning);
                return;
            }

            _qustion?.AddBranch(char.Parse(txtQNum.Text));

            ctrlQustionListView listQustion = new ctrlQustionListView(_qusetionCreater,this);
            listQustion.QuestionIsSelected += Qustion_QuestionStateChanged;

            MainGrid.Visibility= Visibility.Collapsed;
            SubGrid.Visibility= Visibility.Visible;
            SubGrid.Children.Clear();
            SubGrid.Children.Add(listQustion);
        }

        private void Qustion_QuestionStateChanged(object? sender, ( bool IsChecked, string Qustion) e)
        {
            if (!e.IsChecked) 
            {
                foreach (var point in _qustion.PointList)
                {
                    if (point.Text == e.Qustion)
                    {
                        _qustion.PointList.Remove(point);
                        return;
                    }
                }
            
            }
            else
            {
                _qustion?.AddPointToBranch(char.Parse(txtQNum.Text), new clsPoint(e.Qustion));

            }

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed; 
        }
    }
}
