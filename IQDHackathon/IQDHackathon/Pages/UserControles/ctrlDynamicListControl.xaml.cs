using System.Windows.Controls;

namespace Interface.Pages.UserControles
{
    public partial class ctrlDynamicListControl : UserControl
    {
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
                box.StateChanged += Box_StateChanged; 
            }
        }

        private void Box_StateChanged(object sender, (bool IsChecked,string Question) e)
        {
            QuestionStateChanged?.Invoke(this, (e.IsChecked, e.Question));
        }
    }
}
