﻿#pragma checksum "..\..\..\..\Pages\QuestionsStyles.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "E5534B7096EB89D445E98803AF374FFE100AAB0D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using IQD_UI_Library;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Interface.Pages {
    
    
    /// <summary>
    /// QuestionsStyles
    /// </summary>
    public partial class QuestionsStyles : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 15 "..\..\..\..\Pages\QuestionsStyles.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid MainGrid;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\..\Pages\QuestionsStyles.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnClose;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\..\Pages\QuestionsStyles.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRestore;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\..\Pages\QuestionsStyles.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnMinimize;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\..\Pages\QuestionsStyles.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox ItemsListBox;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\..\..\Pages\QuestionsStyles.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal IQD_UI_Library.IQD_Button btnCreateQustion1;
        
        #line default
        #line hidden
        
        
        #line 106 "..\..\..\..\Pages\QuestionsStyles.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid CreateQuestionsPage;
        
        #line default
        #line hidden
        
        
        #line 107 "..\..\..\..\Pages\QuestionsStyles.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Frame ContentFrame;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.3.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Interface;V1.0.0.0;component/pages/questionsstyles.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Pages\QuestionsStyles.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.3.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.MainGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.btnClose = ((System.Windows.Controls.Button)(target));
            
            #line 35 "..\..\..\..\Pages\QuestionsStyles.xaml"
            this.btnClose.Click += new System.Windows.RoutedEventHandler(this.btnClose_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnRestore = ((System.Windows.Controls.Button)(target));
            
            #line 36 "..\..\..\..\Pages\QuestionsStyles.xaml"
            this.btnRestore.Click += new System.Windows.RoutedEventHandler(this.btnRestore_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnMinimize = ((System.Windows.Controls.Button)(target));
            
            #line 48 "..\..\..\..\Pages\QuestionsStyles.xaml"
            this.btnMinimize.Click += new System.Windows.RoutedEventHandler(this.btnMinimize_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 54 "..\..\..\..\Pages\QuestionsStyles.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.BackToMainWindowButton_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.ItemsListBox = ((System.Windows.Controls.ListBox)(target));
            return;
            case 8:
            this.btnCreateQustion1 = ((IQD_UI_Library.IQD_Button)(target));
            
            #line 96 "..\..\..\..\Pages\QuestionsStyles.xaml"
            this.btnCreateQustion1.Click += new System.Windows.RoutedEventHandler(this.btnCreateQustion1_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.CreateQuestionsPage = ((System.Windows.Controls.Grid)(target));
            return;
            case 10:
            this.ContentFrame = ((System.Windows.Controls.Frame)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.3.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 7:
            
            #line 73 "..\..\..\..\Pages\QuestionsStyles.xaml"
            ((IQD_UI_Library.IQD_Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

