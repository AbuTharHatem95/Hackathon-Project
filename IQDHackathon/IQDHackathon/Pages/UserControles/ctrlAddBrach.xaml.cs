using iText.Forms.Xfdf;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Interface.Pages.UserControles
{
    /// <summary>
    /// Interaction logic for ctrlAddBrach.xaml
    /// </summary>
    public partial class ctrlAddBrach : UserControl
    {
        clsQuestion _qustion;
        Dictionary<string, List<string>> dictionary;
        string Qstyles;

        public ctrlAddBrach(Dictionary<string, List<string>> dictionary, string QustionStyle,clsQuestion qustion)
        {
            InitializeComponent();
            this.dictionary = dictionary;
            Qstyles = QustionStyle;
            _qustion= qustion;
        }

       

        private void btnAddPointToBranch_Click(object sender, RoutedEventArgs e)
        {
            _qustion.AddBranch('A');

            QustionListView qustion = new QustionListView(dictionary, Qstyles,null);
            qustion.QuestionStateChanged += Qustion_QuestionStateChanged;



        }

        private void Qustion_QuestionStateChanged(object? sender, (string QuestionStyle, bool IsChecked, byte Score, string Qustion) e)
        {
            if (!e.IsChecked && _qustion?.PointList.Count > 0) 
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
                _qustion.AddPointToBranch('A', new clsPoint(e.Qustion));

            }

        }
    }
}
