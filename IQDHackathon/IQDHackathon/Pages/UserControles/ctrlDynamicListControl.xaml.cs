using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
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
    /// <summary>
    /// Interaction logic for ctrlDynamicListControl.xaml
    /// </summary>
    public partial class ctrlDynamicListControl : UserControl ,IEnumerable
    {
        //هذا الليست الداخلي يحتوي على يوزر كنترول الجيك بوكس


        //public string qustionStyles
        //{
        //    get { return qustionStyles; }
        //    set { qustionStyles = value; }
        //}


        public ctrlDynamicListControl(string QustionStyles,List<string>Qustion)
        {
            InitializeComponent();
           // qustionStyles= QustionStyles;
           txtTitle.Text = QustionStyles;
            LoadDataInUserContol(Qustion);
        }




        private void LoadDataInUserContol(List<string> questionList)
        {
            foreach(var question in questionList)
            {
                ctrlCheckBoxBoxes Box = new ctrlCheckBoxBoxes(question);
                ItemsListBox.Items.Add(Box);
            }

        }












        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }



        // خاصية لتعيين مصدر البيانات
        public IEnumerable<List<string>> Questions
        {
            get { return (IEnumerable<List<string>>)ItemsListBox.ItemsSource; }
            set { ItemsListBox.ItemsSource = value; }
        }

    }
   
}
