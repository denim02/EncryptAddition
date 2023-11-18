using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace EncryptAddition.WPF.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region INotifyPropertyChanged

        // Used to notify the UI of changes and implement data bindings.
        public event PropertyChangedEventHandler? PropertyChanged;

        // OnPropertyChanged should be called by the setter for any Property that is bound to the UI.
        // It automatically infers the property name from the calling setter.
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion


        #region INotifyDataErrorInfo

        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public bool HasErrors => _errors.Count > 0;

        public IEnumerable GetErrors(string? propertyName)
        {
            return _errors.ContainsKey(propertyName) ? _errors[propertyName] : Enumerable.Empty<string>();
        }

        protected void AddError(string errorMessage, [CallerMemberName] string propertyName = null)
        {
            if (!_errors.ContainsKey(propertyName))
                _errors.Add(propertyName, new List<string>());

            _errors[propertyName].Add(errorMessage);
            OnErrorsChanged(propertyName);
        }

        protected void ClearErrors([CallerMemberName] string propertyName = "")
        {
            if (_errors.Remove(propertyName))
                OnErrorsChanged(propertyName);
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public bool IsPropertyValid<TValue>(
          TValue value,
          Func<TValue, (bool IsValid, IEnumerable<string> ErrorMessages)> validationCallback,
          [CallerMemberName] string propertyName = null)
        {
            // Clear previous errors of the current property to be validated 
            ClearErrors(propertyName);

            // Validate using the delegate
            (bool IsValid, IEnumerable<string> ErrorMessages) validationResult = validationCallback?.Invoke(value) ?? (true, Enumerable.Empty<string>());

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.ErrorMessages)
                    AddError(error, propertyName);
            }

            return validationResult.IsValid;
        }
        #endregion
    }
}