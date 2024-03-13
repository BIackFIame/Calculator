using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CalculatorApp
{
    public partial class MainWindow : Window
    {
        private readonly ExpressionManager expressionManager;
        private string result = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            expressionManager = new ExpressionManager();
            DataContext = new CalculatorViewModel(expressionManager);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public string Expression
        {
            get => expressionManager.GetCurrentExpression();
            set => expressionManager.ClearExpression();
        }

        public string Result
        {
            get => result;
            set => SetProperty(ref result, value);
        }
        private void SetProperty(ref string result, string value)
        {
            throw new NotImplementedException();
        }
        private void UpdateResultTextBox()
        {
            Result = expressionManager.Evaluate() ?? string.Empty;
            Expression = expressionManager.GetCurrentExpression();
        }
    }
}