using System.Windows;
using System.Windows.Controls;

namespace Interface.Pages.UserControles
{
    /// <summary>
    /// Interaction logic for QustionListView.xaml
    /// </summary>
    public partial class QustionListView : UserControl
    {
        public event EventHandler<(string QuestionStyle, bool IsChecked, byte Score, string Qustion)>? QuestionStateChanged;

        clsQuestion _question;

       
        string QsutionStyle;
        private Dictionary<string, List<string>> _dictionary;

        private List<string> _QustionList = new List<string>();

        public QustionListView(Dictionary<string, List<string>> dictionary,string QsStyles,clsQuestion qustion)
        {
            InitializeComponent();
           // _dictionary= dictionary;
            QsutionStyle= QsStyles;
            _question = qustion;
            GentetListViewComponat(dictionary,QsStyles);

        }

        private void GentetListViewComponat(Dictionary<string, List<string>> qustiones,string QustionStyles )
        {

            foreach (var style in qustiones)
            {
               
                        // إنشاء عنصر تحكم ديناميكي
                        ctrlDynamicListControl listView = new ctrlDynamicListControl(style.Key, style.Value);

                        // الاشتراك في الحدث الجديد
                        listView.QuestionStateChanged += ListView_QuestionStateChanged;

                        // إضافة العنصر إلى الواجهة
                        ItemsListBox.Items.Add(listView);
               
                
               
            }


        }

        private void ListView_QuestionStateChanged(object sender, (string QuestionStyle, bool IsChecked, byte Score, string Qustion) e)
        {
            // هنا يمكنك معالجة البيانات الواردة
            var questionStyle = e.QuestionStyle;
            var isChecked = e.IsChecked;
            var score = e.Score;
            var Qustion = e.Qustion;

            if(isChecked)
            {
                if (!_QustionList.Contains(e.Qustion))
                {
                    _QustionList.Remove(e.Qustion);

                }
            }
            else
            {
                if(_QustionList.Contains(e.Qustion))
                {
                    _QustionList.Remove(e.Qustion);
                }
            }
            QuestionStateChanged?.Invoke(this, (questionStyle, isChecked, score, Qustion));


            // عرض البيانات في MessageBox (أو حفظها في قائمة أو أي شيء آخر)
            MessageBox.Show($"نمط السؤال: {questionStyle}\nالتحديد: {isChecked}\nالدرجة: {score}\nمحتوى السؤال :{Qustion}");
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

        //يتم ارجاع كل الاسئلة المحدده الى صفحة انشاء الاسئلة
        private void btnAddQustion_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
