using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CalculatorApp
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        private readonly ExpressionManager expressionManager;
        private string expression = string.Empty;
        private string result = string.Empty;
        private string lastResult = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Expression
        {
            get => expression;
            set => SetProperty(ref expression, value);
        }

        public string Result
        {
            get => result;
            set => SetProperty(ref result, value);
        }

        public string LastResult
        {
            get => lastResult;
            set => SetProperty(ref lastResult, value);
        }

        public ICommand AppendCommand { get; }
        public ICommand CalculateCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand BackspaceCommand { get; }

        public CalculatorViewModel(ExpressionManager expressionManager)
        {
            this.expressionManager = expressionManager;

            AppendCommand = new RelayCommand(AppendToExpression);
            CalculateCommand = new RelayCommand(_ => Result = EvaluateExpression());
            ClearCommand = new RelayCommand(ClearExpression);
            BackspaceCommand = new RelayCommand(RemoveLastCharacter);
        }
        private void AppendToExpression(object? parameter)
        {
            string value = parameter?.ToString() ?? string.Empty;

            if (value == "^" && !expressionManager.GetCurrentExpression().EndsWith("^"))
            {
                value = "^(";
            }

            if (Expression == LastResult)
            {
                Expression = string.Empty;
            }

            expressionManager.AppendToExpression(value);
            Expression = expressionManager.GetCurrentExpression();
            Result = EvaluateExpression();
        }

        private string EvaluateExpression()
        {
            string result = expressionManager.Evaluate() ?? string.Empty;
            LastResult = result;
            return result;
        }

        private void ClearExpression(object? _)
        {
            expressionManager.ClearExpression();
            Expression = string.Empty;
            Result = string.Empty;
            LastResult = string.Empty;
        }

        private void RemoveLastCharacter(object? _)
        {
            if (!string.IsNullOrEmpty(Expression))
            {
                expressionManager.RemoveLastCharacter();
                Expression = expressionManager.GetCurrentExpression();
                Result = EvaluateExpression();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}