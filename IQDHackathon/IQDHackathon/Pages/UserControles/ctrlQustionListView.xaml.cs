using System.Windows;
using System.Windows.Controls;

namespace Interface.Pages.UserControles
{
    public partial class QustionListView : UserControl
    {
        public event EventHandler<(string QuestionStyle, bool IsChecked, string Qustion)>? QuestionStateChanged;

        //clsQuestion _question;

        //public static Dictionary<string, List<string>> QuestionsDictFromChatGPT;

        public QustionListView()
        {
            InitializeComponent();
            //GentetListViewComponat();
        }

        //يتم ارجاع كل الاسئلة المحدده الى صفحة انشاء الاسئلة
        private void btnAddQustion_Click(object sender, RoutedEventArgs e)
        {
            QusetionCreater qusetionCreater = new QusetionCreater();
            //qusetionCreater.DataLoaded += LoadDataFromEvent;
            AddQustionGrid.Visibility = Visibility.Collapsed;
            QusetionCreaterGrid.Visibility = Visibility.Visible;
            QusetionCreaterFrame.Navigate(qusetionCreater);
        }

        private void GentetListViewComponat()
        {

            foreach (var question in TestScenarioGeneratorPage.QuestionsDictFromChatGPT)
            {
                //إنشاء عنصر تحكم ديناميكي
                ctrlDynamicListControl listView = new ctrlDynamicListControl(question.Key, question.Value);

                // الاشتراك في الحدث الجديد
                listView.QuestionStateChanged += ListView_QuestionStateChanged;

                ////إضافة العنصر إلى الواجهة
                //ItemsListBox.Items.Add(listView);          // <---------------------------------------@
            }
        }

        private void LoadDataFromEvent(object sender, (string QNum, string Qtitle, string QAnswer, string Qscore) e)
        {
            //هنا نملئ الاوبجكت تبع clsQustion
            clsTitle title = new clsTitle(byte.Parse(e.QNum), byte.Parse(e.Qscore), byte.Parse(e.QAnswer), e.Qtitle);
            _question = new clsQuestion(title);
        }

        private void ListView_QuestionStateChanged(object sender, (bool IsChecked, string Qustion) e)
        {
            // هنا يمكنك معالجة البيانات الواردة
            var isChecked = e.IsChecked;
            var Qustion = e.Qustion;


            if(isChecked)
            {
                if (!_QustionList.Contains(e.Qustion))
                    _QustionList.Remove(e.Qustion);
            }
            else if (_QustionList.Contains(e.Qustion))
                _QustionList.Remove(e.Qustion);
            
            QuestionStateChanged?.Invoke(this, (isChecked, Qustion));

            // عرض البيانات في MessageBox (أو حفظها في قائمة أو أي شيء آخر)
            MessageBox.Show($"{isChecked}\nمحتوى السؤال :{Qustion}");
        }

        private void LoadDataInOBJ(string QText)
        {
            _question.AddPoint(new clsPoint(QText));

            _question.AddBranch('A');
            foreach (var item in _QustionList)
            {
                _question.AddPointToBranch('A', new clsPoint(QText));
            }
        } 
    }
}
