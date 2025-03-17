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


        public event EventHandler<string>? ScoreChanged;
        //public event EventHandler<string> ScoreChanged = delegate { }; // معالج افتراضي

        //private string _qustion;

        //public string question
        //{
        //    get { return _qustion; }
        //    set { _qustion = value; }
        //}

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
            //question = Question;
            checkBox.Content= Question;

        }

        private void txtScore_LostFocus(object sender, RoutedEventArgs e)
        {
            ScoreChanged?.Invoke(this, txtScore.Text);
           // ScoreChanged(this, Text); // لا حاجة لفحص `null`

        }
    }
}
