using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Interface.Pages.UserControles
{
    /// <summary>
    /// Interaction logic for ctrlCheckBoxBoxes.xaml
    /// </summary>
    public partial class ctrlCheckBoxBoxes : UserControl, INotifyPropertyChanged
    {
        public event EventHandler<(bool IsChecked, string Score)>? StateChanged; // حدث جديد للإعلام بحالة CheckBox وقيمة TextBox

        private bool _isCheckedProperty;
        public bool IsCheckedProperty
        {
            get => _isCheckedProperty;
            set
            {
                _isCheckedProperty = value;
                OnPropertyChanged(nameof(IsCheckedProperty));
                MessageBox.Show($"CheckBox حالة التحديد: {(_isCheckedProperty ? "✅ محدد" : "❌ غير محدد")}");
                NotifyStateChanged(); // إشعار عند تغيير حالة CheckBox
            }
        }

        private string _scoreText;
        public string ScoreText
        {
            get => _scoreText;
            set
            {
                _scoreText = value;
                OnPropertyChanged(nameof(ScoreText));
                NotifyStateChanged(); // إشعار عند تغيير قيمة TextBox
            }
        }

        public ctrlCheckBoxBoxes(string question)
        {
            InitializeComponent();
            checkBox.Content = question;
            txtScore.TextChanged += TxtScore_TextChanged; // الاشتراك في حدث تغيير النص
        }

        private void TxtScore_TextChanged(object sender, TextChangedEventArgs e)
        {
            ScoreText = txtScore.Text; // تحديث قيمة ScoreText عند تغيير النص
        }

        private void NotifyStateChanged()
        {
            StateChanged?.Invoke(this, (IsCheckedProperty, ScoreText)); // إرسال الحالة والقيمة
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void txtScore_LostFocus(object sender, RoutedEventArgs e)
        {
            ScoreText = txtScore.Text; // تحديث قيمة ScoreText عند فقدان التركيز
        }
    }
}

