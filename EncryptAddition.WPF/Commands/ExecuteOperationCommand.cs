﻿using EncryptAddition.WPF.Models.ServiceAdapters;
using EncryptAddition.WPF.Models.Stores;
using EncryptAddition.WPF.ViewModels;
using System;
using System.Threading.Tasks;

namespace EncryptAddition.WPF.Commands
{
    public class ExecuteOperationCommand : BaseAsyncCommand
    {
        private readonly EncryptTabViewModel _encryptTabViewModel;
        private IAsyncEncryptionServiceAdapter _encryptionAdapter;

        public ExecuteOperationCommand(EncryptTabViewModel encryptTabViewModel)
        {
            _encryptTabViewModel = encryptTabViewModel;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _encryptTabViewModel.IsPreparingService = true;

            try
            {
                EncryptServiceStore encryptServiceStore;

                encryptServiceStore = _encryptTabViewModel.IsKeyAutoGenerated ? EncryptServiceStore.GetInstance(_encryptTabViewModel.EncryptionChoice, int.Parse(_encryptTabViewModel.BitLength)) : EncryptServiceStore.GetInstance(_encryptTabViewModel.EncryptionChoice, _encryptTabViewModel.SerializedCustomKey);
                _encryptionAdapter = encryptServiceStore.AsyncEncryptionAdapter;

                if (!_encryptionAdapter.IsReady)
                    await _encryptionAdapter.PrepareService();

                _encryptTabViewModel.IsPreparingService = false;
                _encryptTabViewModel.IsRunningOperation = true;

                var results = await (_encryptTabViewModel.OperationChoice == DataTypes.OperationChoice.ENCRYPTION ?
                    _encryptionAdapter.Encrypt(Utils.ParseStringToBigIntegerArray(_encryptTabViewModel.EncryptionInputValues)) :
                    _encryptionAdapter.Decrypt(Utils.ParseStringToCipherTextArray(_encryptTabViewModel.DecryptionInputValues)));

                _encryptTabViewModel.IsRunningOperation = false;
                _encryptTabViewModel.Result = results;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
