using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Interface.Pages.UserControles
{
    /// <summary>
    /// Interaction logic for ctrlDynamicListControl.xaml
    /// </summary>
    public partial class ctrlDynamicListControl : UserControl
    {
        // حدث جديد لنقل البيانات إلى الصفحة الرئيسية
        public event EventHandler<(string QuestionStyle, bool IsChecked, string Score)>? QuestionStateChanged;

        public ctrlDynamicListControl(string questionStyles, List<string> questions)
        {
            InitializeComponent();
            txtTitle.Text = questionStyles;
            LoadDataInUserControl(questions);
        }

        private void LoadDataInUserControl(List<string> questionList)
        {
            foreach (var question in questionList)
            {
                ctrlCheckBoxBoxes box = new ctrlCheckBoxBoxes(question);
                ItemsListBox.Items.Add(box);
                box.StateChanged += Box_StateChanged; // الاشتراك في الحدث الجديد
            }
        }

        private void Box_StateChanged(object sender, (bool IsChecked, string Score) e)
        {
            // هنا يتم نقل البيانات إلى الصفحة الرئيسية
            var questionStyle = txtTitle.Text; // اسم النمط (Style) الحالي
            var isChecked = e.IsChecked;
            var score = e.Score;

            // إطلاق الحدث مع البيانات
            QuestionStateChanged?.Invoke(this, (questionStyle, isChecked, score));
        }


    }




}
