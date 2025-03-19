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
            var isChecked = e.IsChecked;
            var Qustion = e.Qustion;
 
            QuestionIsSelected?.Invoke(this, (isChecked, Qustion));


            //هنا سيتم الغاء هذا المسج بوكس بعد اختبار المشروع
            // عرض البيانات في MessageBox (أو حفظها في قائمة أو أي شيء آخر)
            MessageBox.Show($"{isChecked}\nمحتوى السؤال :{Qustion}");
        }

        private void btnChooesQustion_Click(object sender, RoutedEventArgs e)
        {
            //هنا لا يتم تطبيق اي اجراء فقط اخفاء الليست والرجوع الى صفحة انشاء الاسئلة
            this.Visibility= Visibility.Collapsed;
 
        }
    }
}
