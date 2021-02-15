﻿#pragma checksum "..\..\..\SudokuWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "74BC72EF86808DFAB19CE03735AACE1D7821FBAB"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Sudoku;
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


namespace Sudoku {
    
    
    /// <summary>
    /// SudokuWindow
    /// </summary>
    public partial class SudokuWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\SudokuWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid RootGrid;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\SudokuWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox AlgorithmCBox;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\SudokuWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button LoadBtn;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\SudokuWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CheckBtn;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\SudokuWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SolveBtn;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\SudokuWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button PlaybackBtn;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\SudokuWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer ConsoleScroll;
        
        #line default
        #line hidden
        
        /// <summary>
        /// ConsoleText Name Field
        /// </summary>
        
        #line 40 "..\..\..\SudokuWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public System.Windows.Controls.TextBlock ConsoleText;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\SudokuWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ClearBtn;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\SudokuWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SaveBtn;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\SudokuWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid GameGrid;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.3.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Sudoku;V1.0.0.0;component/sudokuwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\SudokuWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.3.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.RootGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.AlgorithmCBox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 3:
            this.LoadBtn = ((System.Windows.Controls.Button)(target));
            
            #line 34 "..\..\..\SudokuWindow.xaml"
            this.LoadBtn.Click += new System.Windows.RoutedEventHandler(this.LoadBtn_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.CheckBtn = ((System.Windows.Controls.Button)(target));
            return;
            case 5:
            this.SolveBtn = ((System.Windows.Controls.Button)(target));
            
            #line 36 "..\..\..\SudokuWindow.xaml"
            this.SolveBtn.Click += new System.Windows.RoutedEventHandler(this.SolveBtn_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.PlaybackBtn = ((System.Windows.Controls.Button)(target));
            return;
            case 7:
            this.ConsoleScroll = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 8:
            this.ConsoleText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 9:
            this.ClearBtn = ((System.Windows.Controls.Button)(target));
            
            #line 43 "..\..\..\SudokuWindow.xaml"
            this.ClearBtn.Click += new System.Windows.RoutedEventHandler(this.ClearBtn_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.SaveBtn = ((System.Windows.Controls.Button)(target));
            return;
            case 11:
            this.GameGrid = ((System.Windows.Controls.Grid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

