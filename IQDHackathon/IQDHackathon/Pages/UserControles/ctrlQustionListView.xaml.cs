using System.Windows;
using System.Windows.Controls;

namespace Interface.Pages.UserControles
{
   
    public partial class ctrlQustionListView : UserControl
    {
        public event EventHandler<(bool IsChecked, string Qustion)>? QuestionIsSelected;

        //QusetionCreater QusetionCreater;

        public UserControl ParentControl { get; set; }

        public ctrlQustionListView(QusetionCreater qustionpage, ctrlAddBrach ctrlAddbranch=null)
        {
            InitializeComponent();
            GentetListViewComponat();
           // QusetionCreater = qustionpage;

            ParentControl = ctrlAddbranch != null ? (UserControl)ctrlAddbranch : qustionpage;
        }

        private void GentetListViewComponat()
        {
            foreach (var question in TestScenarioGeneratorPage.QuestionsDictFromChatGPT)
            {
                ctrlDynamicListControl listView = new ctrlDynamicListControl(question.Key, question.Value);

                listView.QuestionStateChanged += ListView_QuestionStateChanged;

                ItemsListBox.Items.Add(listView);
            }
        }

        private void ListView_QuestionStateChanged(object? sender, (bool IsChecked, string Qustion) e)
        {
            QuestionIsSelected?.Invoke(this, (e.IsChecked, e.Qustion));
        }

        private void btnChooesQustion_Click(object sender, RoutedEventArgs e)
        {

            this.Visibility = Visibility.Collapsed;

            if (ParentControl is QusetionCreater qustionCreater)
            {
                if (qustionCreater.MainGrid.Visibility == Visibility.Collapsed)
                    qustionCreater.MainGrid.Visibility = Visibility.Visible;

                qustionCreater.SubGrid.Visibility = Visibility.Collapsed;
            }
            else if (ParentControl is ctrlAddBrach addBrach)
            {
                if (addBrach.MainGrid.Visibility == Visibility.Collapsed)
                    addBrach.MainGrid.Visibility = Visibility.Visible;

                addBrach.SubGrid.Visibility = Visibility.Collapsed;
            }

            //this.Visibility = Visibility.Collapsed;

            //if (QusetionCreater.MainGrid.Visibility == Visibility.Collapsed)
            //    QusetionCreater.MainGrid.Visibility = Visibility.Visible;

            //QusetionCreater.SubGrid.Visibility = Visibility.Collapsed;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {

            this.Visibility = Visibility.Collapsed;

            if (ParentControl is QusetionCreater qustionCreater)
            {
                if (qustionCreater.MainGrid.Visibility == Visibility.Collapsed)
                    qustionCreater.MainGrid.Visibility = Visibility.Visible;

                qustionCreater.SubGrid.Visibility = Visibility.Collapsed;
            }
            else if (ParentControl is ctrlAddBrach addBrach)
            {
                if (addBrach.MainGrid.Visibility == Visibility.Collapsed)
                    addBrach.MainGrid.Visibility = Visibility.Visible;

                addBrach.SubGrid.Visibility = Visibility.Collapsed;
            }

            //this.Visibility= Visibility.Collapsed;
            //if (QusetionCreater.MainGrid.Visibility == Visibility.Collapsed)
            //    QusetionCreater.MainGrid.Visibility = Visibility.Visible;

            //QusetionCreater.SubGrid.Visibility = Visibility.Collapsed;
        }
    }
}
