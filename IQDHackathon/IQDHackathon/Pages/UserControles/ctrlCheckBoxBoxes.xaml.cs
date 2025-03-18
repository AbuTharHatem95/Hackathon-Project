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
        public event EventHandler<(bool IsChecked, byte Score,string Qustion)>? StateChanged; // حدث جديد للإعلام بحالة CheckBox وقيمة TextBox

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
                NotifyScoreChanged(); // إشعار عند تغيير قيمة TextBox
            }
        }

        private string _question {  get; set; }

        public ctrlCheckBoxBoxes(string question)
        {
            InitializeComponent();
            checkBox.Content = question;
            _question= question;
           // txtScore.LostFocus += txtScore_LostFocus; // الاشتراك في حدث تغيير النص
            this.DataContext = this; // تعيين DataContext إلى نفس الكلاس

        }

        //private void TxtScore_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    ScoreText = txtScore.Text; // تحديث قيمة ScoreText عند تغيير النص
        //}

        private void NotifyStateChanged()
        {
            if(IsCheckedProperty)
            {
               // txtScore.Focus();
                StateChanged?.Invoke(this, (IsCheckedProperty, byte.Parse(ScoreText), _question)); // إرسال الحالة والقيمة

            }
            
            //StateChanged?.Invoke(this, (IsCheckedProperty, ScoreText, _question)); // إرسال الحالة والقيمة



        }

        private void NotifyScoreChanged()
        {
            if (!IsCheckedProperty)
            {
                IsCheckedProperty = true;

                StateChanged?.Invoke(this, (IsCheckedProperty, byte.Parse(ScoreText), _question)); // إرسال الحالة والقيمة

            }
            else if (txtScore.Text != "")
            {
                StateChanged?.Invoke(this, (IsCheckedProperty,byte.Parse(ScoreText), _question)); // إرسال الحالة والقيمة

            }
            //StateChanged?.Invoke(this, (IsCheckedProperty, ScoreText, _question)); // إرسال الحالة والقيمة



        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //private void txtScore_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    if (txtScore.Text =="")
        //    {

        //    }
        //    else
        //        ScoreText = txtScore.Text; // تحديث قيمة ScoreText عند فقدان التركيز
        //}
    }
}

