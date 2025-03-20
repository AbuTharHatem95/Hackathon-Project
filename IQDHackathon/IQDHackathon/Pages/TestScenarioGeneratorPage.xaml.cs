﻿using BLL;
using Interface.LogicClasses;
using Interface.Pages.UserControles;
using IQD_UI_Library;
using IQDHackathon;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;


namespace Interface.Pages
{
    //يجب العمل على ان يجلب الاسئلة بناء على النمط المختار من الواجهه وبناء عليها يصممها الجات جي بي تي 
    //بيج لاملائ معلومات المدرسه ومعلومات الاستاذ ورفع مادة الامتحان 
    public partial class TestScenarioGeneratorPage : Page
    {
        private readonly string? __openAiApiKey =  Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        public List<StyleModel> Styles { get; set; }

        public static Dictionary<string, List<string>> QuestionsDictFromChatGPT = new Dictionary<string, List<string>>();

        private string extractedText;

        IQD_LoadingControl load;


        public TestScenarioGeneratorPage()
        {
            InitializeComponent();
            FillComboBox();

        }

        private void FillQuestionTypes(int subjectId)
        {
            DataTable dt = clsQuestionsType.GetAllQuestionTypesBySubject(subjectId);
            Styles = new List<StyleModel>();    
            
            foreach (DataRow dr in dt.Rows)
            {
                Styles.Add(new StyleModel { Name = dr[0].ToString()!, IsSelected = false });
            }

            CheckBoxList.ItemsSource = Styles; 

        }

        private bool VauldationText()
        {
            if (
                string.IsNullOrEmpty(txtNote.Text) ||
                string.IsNullOrEmpty(CombClass.Text) ||
                string.IsNullOrEmpty(CombGrade.Text) ||
                string.IsNullOrEmpty(CombStage.Text) ||
                string.IsNullOrEmpty(txtSchoolName.Text) ||
                string.IsNullOrEmpty(txtExampleTime.Text) ||
                string.IsNullOrEmpty(txtTeacherName.Text) ||
                string.IsNullOrEmpty(txtTypeQuze.Text))
            {
                IQD_MessageBox.Show("تحذير", "يجب ملئ المعلومات", MessageBoxType.Warning);
                return true;
            }
            return false;
        }

        private void FillComboBox()
        {
            // إضافة عناصر إلى ComboBox
            FillComboBoxStage();
            FillComboBoxSubject((int)CombStage.SelectedValue);
            FillQuestionTypes((int)CombSubject.SelectedValue);

        }

        //انشاء نموذج بشكل دينمايكي باستخدام GPT
        private async void GenretWithGPT_Click(object sender, RoutedEventArgs e)
        {
            //if (VauldationText())
            //    return;

            IQD_MessageBox.Show("اختيار", "يرجى اختيار ملف المادة");

           // await LoadPdfFile();
        }


        // بناء نموذج اسئلة بشكل يدوي 
        private async void Gentet_Click(object sender, RoutedEventArgs e)
        {
            if (VauldationText())
                return;

            var selectedStyles = Styles.Where(style => style.IsSelected).Select(style => style.Name).ToList();
            QuestionsDictFromChatGPT = await clsPdfManipulation.GenerateQuestionsFromPdfUsingAiGpt("", selectedStyles);
            MainPageGrid.Visibility = Visibility.Collapsed;
            SubMain.Visibility = Visibility.Visible;
            SubMain.Children.Clear();
            SubMain.Children.Add(new AddQustiones(this));
        }


        private void FillComboBoxStage()
        {
            CombStage.ItemsSource = clsStage.GetAll().DefaultView;
            CombStage.DisplayMemberPath = "StageName";
            CombStage.SelectedValuePath = "StageId";

            CombStage.SelectedIndex = 0;
        }

        private void FillComboBoxSubject(int stageId)
        {
            CombSubject.ItemsSource = null;
            CombSubject.Items.Clear();
            CombSubject.ItemsSource = clsSubject.GetAllSubjectsByStage(stageId).DefaultView;
            CombSubject.DisplayMemberPath = "SubjectName";
            CombSubject.SelectedValuePath = "SubjectId";
            CombSubject.SelectedIndex = 0;
        }

        //اجراء خاص بجات جي بي تي ,حيث يتم بناء النموذج بشكل يدوي مع تمرير انماط الاسئلة

        // يستخدم بتعبة النموذج الاسئلة هارد كود  

        //يستخدم مع جات جي بي تي 
        public void GeneratePdf(ref string fullPath)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            DateTime curruntYear = DateTime.Now;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin((float)0.4, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Content().PaddingVertical(5).Column(column =>
                    {
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().AlignLeft().Text($"المادة: {CombSubject.Text}\n\nالتاريخ: {txtExapleDate.Text}\n\nالوقت: {txtExampleTime.Text}\n").Bold().FontSize(12);
                            row.RelativeItem().AlignCenter().Text($"{curruntYear.Year}/{curruntYear.AddYears(-1)})\n\nنوع الأمتحان: {txtTypeQuze.Text}\n\n{txtNote.Text}").Bold().FontSize(12);
                            row.RelativeItem().AlignRight().Text($"المدرسة: {txtSchoolName.Text}\n\n:مدرس المادة {txtTeacherName.Text}\n").Bold().FontSize(12);
                        });

                        column.Item().LineHorizontal(2).LineColor(Colors.Black);
                        column.Item().PaddingVertical(10);

                        foreach (var questionEntry in clsQuestion.QuestionsDict)
                        {
                            var question = questionEntry.Value;
                            column.Item().Row(row =>
                            {
                                row.RelativeItem().AlignRight().Text($"س{question.Title.Number}: {question.Title.QuestionTitle} (درجة {question.Title.ScoreForBranchOrPint})").Bold().FontSize(12);
                            });

                            if (question.BranchzDict != null)
                            {
                                foreach (var branchEntry in question.BranchzDict)
                                {
                                    var branch = branchEntry.Value;
                                    column.Item().Row(row =>
                                    {
                                        row.RelativeItem().AlignRight().Text($"({branch.Char}) {branch.BranchTitle} (درجة {branch.Score})").Bold().FontSize(12);
                                    });

                                    foreach (var point in branch.PointList)
                                    {
                                        column.Item().Row(row =>
                                        {
                                            row.RelativeItem().AlignRight().Text($"- {point.Text} (درجة {point.Score})").FontSize(12);
                                        });
                                    }
                                    // إضافة سطر فارغ بين الفروع
                                    column.Item().PaddingVertical(5);
                                }
                            }

                            if (question.PointList != null)
                            {
                                foreach (var point in question.PointList)
                                {
                                    column.Item().Row(row =>
                                    {
                                        row.RelativeItem().AlignRight().Text($"- {point.Text} (درجة {point.Score})").FontSize(12);
                                    });
                                }
                            }
                            // إضافة سطرين فارغين قبل الخط الأسود
                            column.Item().PaddingVertical(10);
                            column.Item().LineHorizontal(2).LineColor(Colors.Black);
                            column.Item().PaddingVertical(10);
                        }
                    });
                });
            })
            .GeneratePdf(fullPath);
        }
       
        //اجراءات خاصة بلصفحة
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // إغلاق الصفحة إذا كانت معروضة داخل NavigationWindow أو Frame
            if (this.NavigationService != null)
                this.NavigationService.GoBack(); // العودة إلى الصفحة السابقة
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            // الوصول إلى النافذة التي تحتوي على الصفحة
            var window = Window.GetWindow(this);
            if (window != null)
            {
                // تغيير حالة النافذة بين التكبير والتصغير
                if (window.WindowState == WindowState.Normal)
                    window.WindowState = WindowState.Maximized;
                else
                    window.WindowState = WindowState.Normal;
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            // الوصول إلى النافذة التي تحتوي على الصفحة
            var window = Window.GetWindow(this);
            if (window != null)
                window.WindowState = WindowState.Minimized; // تصغير النافذة
        }

        private void btnRetuntomainmenue_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Window.GetWindow(this).Close();
        }

        private void CombStage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // FillComboBoxSubject(Convert.ToInt32(CombStage.SelectedValuePath));
           
           FillComboBoxSubject((int)CombStage.SelectedValue);
        }

        private void txtSchoolName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void CombStage_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void CombSubject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(CombSubject.SelectedValue != null)
                FillQuestionTypes((int)CombSubject.SelectedValue);
        }
    }

    public class StyleModel : INotifyPropertyChanged
    {
        private string _name;

        private bool _isSelected;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

   
}