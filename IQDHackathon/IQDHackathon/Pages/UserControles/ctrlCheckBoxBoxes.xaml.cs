using System.ComponentModel;
using System.Windows.Controls;

namespace Interface.Pages.UserControles
{
    public partial class ctrlCheckBoxBoxes : UserControl, INotifyPropertyChanged
    {
        public event EventHandler<(bool IsChecked,string Qustion)>? StateChanged;

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
                NotifyStateChanged(); 
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
           StateChanged?.Invoke(this, (IsCheckedProperty, _question)); 
        }
       
    }
}

