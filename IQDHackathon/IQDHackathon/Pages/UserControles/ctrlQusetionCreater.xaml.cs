using System.Windows;
using System.Windows.Controls;

namespace Interface.Pages.UserControles
{
    public partial class QusetionCreater : UserControl
    {
        public clsQuestion Questions;

        public event EventHandler<(bool IsChecked, string Qustion)>? StateChanged; // حدث جديد للإعلام بحالة CheckBox وقيمة TextBox

        public event EventHandler<(string QustionNum, string QustionTitle, string QustionScor, string NumberOfAnswer)>? DataLoaded;

        public QusetionCreater()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void btnAddQustion_Click(object sender, RoutedEventArgs e)
        {
            clsTitle title = new clsTitle(byte.Parse(txtQNum.Text), byte.Parse(txtQscore.Text), byte.Parse(txtNumberOfAnswers.Text), txtQustionTitle.Text);
            Questions = new clsQuestion(title);
        }

        private void btnAddQustionPointes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoadDataFromEvent(object sender, (string QNum, string Qtitle, string QAnswer, string Qscore) e)
        {
            //هنا نملئ الاوبجكت تبع clsQustion
        }




        //private void LoadDataFromText()
        //{
        //    question = new clsQuestion(new clsTitle(byte.Parse(txtQNum.Text),byte.Parse(txtQscore.Text),byte.Parse(txtNumberOfAnswers.Text),txtQustionTitle.Text));
        //}

        //private void btnAddNewBrach_Click(object sender, RoutedEventArgs e)
        //{

        //    QustionCreate.Visibility = Visibility.Collapsed;
        //    QustionList.Visibility = Visibility.Visible;
        //    ContentFrame.Navigate(new ctrlAddBrach(QuestionsDictFromChatGPT, QStyle, question));

        //   // ContentFrame.Navigate(new QustionListView(_qustion, QustionListView.Mod.BrachMod,QStyle,question)); 
        //    //يجب فتح الليست بدون النمط الوقف عليه
        //}

        //private void btnAddQustion_Click(object sender, RoutedEventArgs e)
        //{
        //    LoadDataFromText();

        //    QustionCreate.Visibility = Visibility.Collapsed;
        //    QustionList.Visibility = Visibility.Visible;
        //    ContentFrame.Navigate(new QustionListView(QuestionsDictFromChatGPT,QStyle,question)); 
        //}

        ////يتم انشاء اوبجكت عند حدوث هذا الايفنت
        //private void btnCreateQustion_Click(object sender, RoutedEventArgs e)
        //{
        //    //انشاء اوبجكت من التايتل 
        //    //باي بوتن تريد يصير هذا الايفنت تضيف هذا السطر
        //    DataLoaded?.Invoke(this, (txtQNum.Text, txtQustionTitle.Text, txtQscore.Text, txtNumberOfAnswers.Text));
        //}

        //private void GentetListViewComponat(Dictionary<string, List<string>> qustiones)
        //{
        //    foreach (var style in qustiones)
        //    {
        //        // إنشاء عنصر تحكم ديناميكي
        //        ctrlDynamicListControl listView = new ctrlDynamicListControl(style.Key, style.Value);

        //        // الاشتراك في الحدث الجديد
        //        //listView.QuestionStateChanged += ListView_QuestionStateChanged;

        //        //// إضافة العنصر إلى الواجهة
        //        //ItemsListBox.Items.Add(listView);
        //    }
        //}
    }
}
