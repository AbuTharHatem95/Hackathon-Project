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
                ItemsListBox.Items.Add(question.Key);

                foreach (var item in question.Value)
                {
                    ItemsListBox.Items.Add(item);
                }

               
            }
        }

        // اختيار اسئلة بشكل يدوي
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
            //TestScenarioGeneratorPage.QuestionsDictFromChatGPT = clsGptManipulation.QuestionsWithTypes()
        }
    }
}
