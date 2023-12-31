﻿using EncryptAddition.Crypto;
using EncryptAddition.WPF.Commands;
using EncryptAddition.WPF.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace EncryptAddition.WPF.ViewModels
{
    public class EncryptTabViewModel : BaseViewModel
    {
        #region Data Members
        // Property used to automatically populate combobox
        public EncryptionChoice[] StrategyList { get; } = { EncryptionChoice.ElGamal, EncryptionChoice.Paillier };

        #region Input Fields
        // Key Generation Choice radio boxes
        private bool _isKeyAutoGenerated = true;
        public bool IsKeyAutoGenerated
        {
            get => _isKeyAutoGenerated;
            set
            {
                _isKeyAutoGenerated = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanDecrypt));
                OnPropertyChanged(nameof(IsDataValid));
            }
        }

        // BitLength textbox field
        private string _bitLength;
        private bool IsValidBitLength { get; set; } = false;
        private bool _wasWarned = false;
        public string BitLength
        {
            get => _bitLength;
            set
            {
                IsValidBitLength = IsPropertyValid(value, IsBitLengthValid);

                if (int.TryParse(value, out int result) && result > 256 && !_wasWarned)
                {
                    MessageBox.Show($"The chosen bit length might produce significant delays in generating results. Values under 256 bits are recommended.", "Bit Length Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    _wasWarned = true;
                }

                _bitLength = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDataValid));
            }
        }
        // BitLength validation logic
        private (bool IsValid, IEnumerable<string> ErrorMessages) IsBitLengthValid(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return (false, new[] { "The bit length field is required." });

            // Try to parse the bit length to an int
            if (int.TryParse(value, out int bitLength))
            {
                if (bitLength < 3)
                    return (false, new[] { "The bit length value must be 3 or greater." });
                else
                    return (true, Enumerable.Empty<string>());
            }
            return (false, new[] { "The bit length value must be a positive integer." });
        }

        // Custom Key textbox field
        private string _serializedCustomKey;
        public bool IsValidSerializedCustomKey { get; set; } = false;
        public string SerializedCustomKey
        {
            get => _serializedCustomKey;
            set
            {
                IsValidSerializedCustomKey = IsPropertyValid(value, IsCustomKeyValid);

                _serializedCustomKey = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDataValid));
            }
        }
        // Custom Key validation logic (based on chosen algorithm)
        private (bool IsValid, IEnumerable<string> ErrorMessages) IsCustomKeyValid(string value)
        {
            // Check if the field is empty
            if (String.IsNullOrWhiteSpace(value))
                return (false, new[] { "The custom key field is required." });

            if ((EncryptionChoice == EncryptionChoice.ElGamal) ?
                Crypto.ElGamal.KeyPair.ValidateSerializedKeys(value) :
                Crypto.Paillier.KeyPair.ValidateSerializedKeys(value))
                return (true, Enumerable.Empty<string>());
            else
                return (false, new[] { "Invalid custom key pair format. A " + EncryptionChoice + " key pair must have the format: " + (EncryptionChoice == EncryptionChoice.Paillier ? "111|123;123|123." : "111|123|123;123.") });
        }

        // Algorithm Choice combobox field
        private EncryptionChoice _encryptionChoice = EncryptionChoice.Paillier;
        public EncryptionChoice EncryptionChoice
        {
            get => _encryptionChoice;
            set
            {
                _encryptionChoice = value;
                OnPropertyChanged();
                IsValidSerializedCustomKey = IsPropertyValid(SerializedCustomKey, IsCustomKeyValid, nameof(SerializedCustomKey));
                OnPropertyChanged(nameof(IsDataValid));
            }
        }

        // Field to see if decryption is possible (aka. key is not auto-generated)
        public bool CanDecrypt => !IsKeyAutoGenerated;

        // Operation choice radio button field
        private OperationChoice _operationChoice = OperationChoice.ENCRYPTION;
        public OperationChoice OperationChoice
        {
            get => _operationChoice;
            set
            {
                _operationChoice = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDataValid));
            }
        }

        // Input textbox field for encryption
        private string _encryptionInputValues;
        public bool IsValidEncryptionInputValues { get; set; } = false;
        public string EncryptionInputValues
        {
            get => _encryptionInputValues;
            set
            {
                IsValidEncryptionInputValues = IsPropertyValid(value, IsEncryptionInputValuesValid);

                _encryptionInputValues = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDataValid));
            }
        }
        // Validation logic for encryption input
        private readonly Regex _encryptionInputValueFormat = new Regex("^\\s*\\d+(\\s*,\\s*\\d+\\s*)*$");
        private (bool IsValid, IEnumerable<string> ErrorMessages) IsEncryptionInputValuesValid(string value)
        {
            // Check if the field is empty
            if (String.IsNullOrWhiteSpace(value))
                return (false, new[] { "The input value field is required." });

            if (_encryptionInputValueFormat.IsMatch(value))
                return (true, Enumerable.Empty<string>());
            else
                return (false, new[] { "Invalid format. The encryption input values must be entered as a comma-separated list of positive integers." });
        }

        // Input textbox field for decryption
        private string _decryptionInputValues;
        public bool IsValidDecryptionInputValues { get; set; } = false;
        public string DecryptionInputValues
        {
            get => _decryptionInputValues;
            set
            {
                IsValidDecryptionInputValues = IsPropertyValid(value, IsDecryptionInputValuesValid);

                _decryptionInputValues = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDataValid));
            }
        }
        // Validation logic for decryption input
        private readonly Regex _decryptionInputValueFormat = new Regex("^(\\d+(\\|\\d+)?)(\\s*,\\s*\\d+(\\|\\d+)?)*$");
        private (bool IsValid, IEnumerable<string> ErrorMessages) IsDecryptionInputValuesValid(string value)
        {
            // Check if the field is empty
            if (String.IsNullOrWhiteSpace(value))
                return (false, new[] { "The input value field is required." });

            if (_decryptionInputValueFormat.IsMatch(value))
                return (true, Enumerable.Empty<string>());
            else
                return (false, new[] { "Invalid format. The input values must be entered as a comma-separated list of ciphertexts." });
        }

        // Used to block the run button if the inputs are invaid
        public bool IsDataValid => ((IsKeyAutoGenerated && IsValidBitLength) ||
            (!IsKeyAutoGenerated && IsValidSerializedCustomKey)) &&
            ((OperationChoice == OperationChoice.ENCRYPTION && IsValidEncryptionInputValues) ||
            (OperationChoice == OperationChoice.DECRYPTION && IsValidDecryptionInputValues));
        #endregion

        #region Output Fields
        private EncryptionServiceResult? _result = null;
        public EncryptionServiceResult? Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Command Execution fields
        private bool _isPreparingService = false;
        public bool IsPreparingService
        {
            get => _isPreparingService;
            set
            {
                _isPreparingService = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        private bool _isRunningOperation = false;
        public bool IsRunningOperation
        {
            get => _isRunningOperation;
            set
            {
                _isRunningOperation = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public bool IsBusy => IsPreparingService || _isRunningOperation;

        public ICommand ExecuteOperation { get; }
        #endregion
        #endregion

        // Constructor
        public EncryptTabViewModel()
        {
            ExecuteOperation = new ExecuteOperationCommand(this);
        }
    }
}
