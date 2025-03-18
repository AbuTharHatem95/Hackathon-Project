using System.Windows.Controls;

namespace Interface.Pages.UserControles
{
    public partial class ctrlDynamicListControl : UserControl
    {
        // حدث جديد لنقل البيانات إلى الصفحة الرئيسية
        public event EventHandler<(bool IsChecked,string Qustion)>? QuestionStateChanged;

        public ctrlDynamicListControl(string questionStyles, List<string> questionList)
        {
            InitializeComponent();
            txtTitle.Text = questionStyles;
            LoadDataInUserControl(questionList);
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

        private void Box_StateChanged(object sender, (bool IsChecked,string Question) e)
        {
            // هنا يتم نقل البيانات إلى الصفحة الرئيسية
            var questionStyle = txtTitle.Text; // اسم النمط (Style) الحالي
            var isChecked = e.IsChecked;
            var Qustion = e.Question;

            // إطلاق الحدث مع البيانات
            QuestionStateChanged?.Invoke(this, (isChecked, Qustion));
        }
    }
}
