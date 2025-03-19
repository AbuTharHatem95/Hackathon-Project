using System.Windows;
using System.Windows.Controls;

namespace Interface.Pages.UserControles
{
   
    public partial class ctrlQustionListView : UserControl
    {
        public event EventHandler<(bool IsChecked, string Qustion)>? QuestionIsSelected; 

        public ctrlQustionListView()
        {
            InitializeComponent();
            GentetListViewComponat();
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
            //هنا لا يتم تطبيق اي اجراء فقط اخفاء الليست والرجوع الى صفحة انشاء الاسئلة
            this.Visibility= Visibility.Collapsed;

 
        }
    }
}
