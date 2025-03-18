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
        public event EventHandler<(bool IsChecked,string Qustion)>? StateChanged; // حدث جديد للإعلام بحالة CheckBox وقيمة TextBox

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                NotifyStateChanged(); // إشعار عند تغيير حالة CheckBox
            }
        }

        private string _question {  get; set; }

        public ctrlCheckBoxBoxes(string question)
        {
            InitializeComponent();
            checkBox.Content = question;
            _question= question;
            this.DataContext = this; 

        }

        private void NotifyStateChanged()
        {
            if(IsCheckedProperty)
            {
                StateChanged?.Invoke(this, (IsCheckedProperty, _question)); // إرسال الحالة والقيمة

            }
      
        }

       


       
    }
}

