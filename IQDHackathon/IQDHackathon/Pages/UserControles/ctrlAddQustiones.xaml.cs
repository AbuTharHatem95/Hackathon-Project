using System.Windows;
using System.Windows.Controls;

namespace Interface.Pages.UserControles
{
    public partial class AddQustiones : UserControl
    {
        public event EventHandler<(bool IsChecked, string Qustion)>? QuestionIsSelected;


        TestScenarioGeneratorPage testpage;

        public AddQustiones(TestScenarioGeneratorPage TestPage)
        {
            InitializeComponent();
            this.testpage = TestPage;

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


        //اختيار اسئلة بشكل يدوي
        private void btnAddQustion_Click(object sender, RoutedEventArgs e)
        {
            QusetionCreater qusetionCreater = new QusetionCreater(ref testpage);
            MainGrid.Visibility = Visibility.Collapsed;
            SubGrid.Visibility = Visibility.Visible;
            SubGrid.Children.Add(qusetionCreater);
        }

        //اعادة توليد اسئلة
        private void btnRelodGenric_Click(object sender, RoutedEventArgs e)
        {
            //هنا يكتب كود ليعيد ملئ دكشنري الاسئلة من خلال جات جي بي تي 
        }



    }
}
