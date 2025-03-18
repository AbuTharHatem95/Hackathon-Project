using Interface;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace Interface.Pages.UserControles
{
    /// <summary>
    /// Interaction logic for QusetionCreater.xaml
    /// </summary>
    
    
    public partial class QusetionCreater : UserControl
    {
        clsQuestion question;

        private Dictionary<string, List<string>> _qustion;

        private string QStyle;

        public QusetionCreater(Dictionary<string, List<string>> dictionary,string QustionStyle)
        {
            InitializeComponent();
            this.DataContext = this;
            _qustion= dictionary;
            QStyle= QustionStyle;
        }


        private void LoadDataFromText()
        {
            question = new clsQuestion(new clsTitle(byte.Parse(txtQNum.Text),byte.Parse(txtQscore.Text),byte.Parse(txtNumberOfAnswers.Text),txtQustionTitle.Text));

        }

        private void btnAddNewBrach_Click(object sender, RoutedEventArgs e)
        {

            QustionCreate.Visibility = Visibility.Collapsed;
            QustionList.Visibility = Visibility.Visible;
            ContentFrame.Navigate(new ctrlAddBrach(_qustion, QStyle, question));

           // ContentFrame.Navigate(new QustionListView(_qustion, QustionListView.Mod.BrachMod,QStyle,question)); 

            //يجب فتح الليست بدون النمط الوقف عليه
        }

        private void btnAddQustion_Click(object sender, RoutedEventArgs e)
        {
            LoadDataFromText();




            QustionCreate.Visibility = Visibility.Collapsed;
            QustionList.Visibility = Visibility.Visible;
            ContentFrame.Navigate(new QustionListView(_qustion,QStyle,question)); 

        }


        //يتم انشاء اوبجكت عند حدوث هذا الايفنت
        private void btnCreateQustion_Click(object sender, RoutedEventArgs e)
        {
            //انشاء اوبجكت من التايتل 
        }







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
