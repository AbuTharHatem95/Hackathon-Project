using System.Windows;
using System.Windows.Controls;

namespace Interface.Pages.UserControles
{
   
    public partial class ctrlQustionListView : UserControl
    {
        public event EventHandler<(bool IsChecked, string Qustion)>? QuestionIsSelected;

        QusetionCreater QusetionCreater;
        ctrlAddBrach _ctrlAddBrach;

        // خاصية لتحديد من استدعى الـ UserControl
        public UserControl ParentControl { get; set; }

        public ctrlQustionListView(QusetionCreater qustionpage,ctrlAddBrach ctrlAddbranch=null)
        {
            InitializeComponent();
            GentetListViewComponat();
            QusetionCreater = qustionpage;
            _ctrlAddBrach = ctrlAddbranch;
            /// تحديد ParentControl بناءً على من استدعى الـ UserControl
            ParentControl = ctrlAddbranch != null ? (UserControl)ctrlAddbranch : qustionpage;
        }

        private void GentetListViewComponat()
        {
            foreach (var question in TestScenarioGeneratorPage.QuestionsDictFromChatGPT)
            {
                //إنشاء عنصر تحكم الليست فيو التي تحتوي على كنترول الجيك بوكس
                ctrlDynamicListControl listView = new ctrlDynamicListControl(question.Key, question.Value);

                // الاشتراك في الحدث 
                listView.QuestionStateChanged += ListView_QuestionStateChanged;

                //إضافة العنصر إلى الواجهة
                ItemsListBox.Items.Add(listView);
            }
        }

        private void ListView_QuestionStateChanged(object? sender, (bool IsChecked, string Qustion) e)
        {
            QuestionIsSelected?.Invoke(this, (e.IsChecked, e.Qustion));


            //هنا سيتم الغاء هذا المسج بوكس بعد اختبار المشروع
            MessageBox.Show($"{e.IsChecked}\nمحتوى السؤال :{e.Qustion}");
        }

        private void btnChooesQustion_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;

            if (QusetionCreater.MainGrid.Visibility == Visibility.Collapsed)
                QusetionCreater.MainGrid.Visibility = Visibility.Visible;

            QusetionCreater.SubGrid.Visibility = Visibility.Collapsed;



        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility= Visibility.Collapsed;
            if (QusetionCreater.MainGrid.Visibility == Visibility.Collapsed)
                QusetionCreater.MainGrid.Visibility = Visibility.Visible;

            QusetionCreater.SubGrid.Visibility = Visibility.Collapsed;

        }
    }
}
