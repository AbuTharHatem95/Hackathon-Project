using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Interface.Pages.UserControles
{
   
    public partial class ctrlQustionListView : UserControl
    {
        public event EventHandler<(bool IsChecked, string Qustion)>? QuestionStateChanged; 

        public ctrlQustionListView()
        {
            InitializeComponent();
            GentetListViewComponat();
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
                ItemsListBox.Items.Add(listView);
            }
        }

        private void ListView_QuestionStateChanged(object sender, (bool IsChecked, string Qustion) e)
        {
            var isChecked = e.IsChecked;
            var Qustion = e.Qustion;


            //if (isChecked)
            //{
            //    if (!_QustionList.Contains(e.Qustion))
            //        _QustionList.Remove(e.Qustion);
            //}
            //else if (_QustionList.Contains(e.Qustion))
            //    _QustionList.Remove(e.Qustion);

            QuestionStateChanged?.Invoke(this, (isChecked, Qustion));

            // عرض البيانات في MessageBox (أو حفظها في قائمة أو أي شيء آخر)
            MessageBox.Show($"{isChecked}\nمحتوى السؤال :{Qustion}");
        }

        private void btnChooesQustion_Click(object sender, RoutedEventArgs e)
        {
            QusetionCreater CreateQustion = new QusetionCreater();
            MainGrid.Visibility = Visibility.Collapsed;
            SubGrid.Visibility = Visibility.Visible;
            ContentFrame.Navigate(CreateQustion);

        }
    }
}
