using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Interface.Pages.UserControles
{
    /// <summary>
    /// Interaction logic for ctrlCheckBoxBoxes.xaml
    /// </summary>
    public partial class ctrlCheckBoxBoxes : UserControl ,INotifyPropertyChanged
    {
        //نص السؤال
        //الدرجة
        //هل تم تحديدها


        public event EventHandler<string>? ValueChanged;
        //public event EventHandler<string> ValueChanged = delegate { }; // معالج افتراضي


        public string question
        {
            get { return question; }
            set { question = value; }
        }

        private bool _isCheckedProperty;
        public bool IsCheckedProperty
        {
            get => _isCheckedProperty;
            set
            {
                _isCheckedProperty = value;
                OnPropertyChanged(nameof(IsCheckedProperty));
                MessageBox.Show($"CheckBox حالة التحديد: {(_isCheckedProperty ? "✅ محدد" : "❌ غير محدد")}");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ctrlCheckBoxBoxes(string Question)
        {
            InitializeComponent();
            question = Question;

        }

        private void txtScore_LostFocus(object sender, RoutedEventArgs e)
        {
            ValueChanged?.Invoke(this, txtScore.Text);
           // ValueChanged(this, Text); // لا حاجة لفحص `null`

        }
    }
}
