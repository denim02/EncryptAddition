﻿using EncryptAddition.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace EncryptAddition.WPF.Views
{
    /// <summary>
    /// Interaction logic for EncryptTabView.xaml
    /// </summary>
    public partial class EncryptTabView : UserControl
    {
        public EncryptTabView()
        {
            InitializeComponent();
            DataContext = new EncryptTabViewModel();
        }

        public void KeyChoiceRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // Set IsKeyAutoGenerated property in view model based on which radio button is checked
            var radioButton = (RadioButton)sender;
            var viewModel = (EncryptTabViewModel)DataContext;
            viewModel.IsKeyAutoGenerated = radioButton.Name == "autoKeyRadioButton";
        }

        public void OperationChoiceRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // Set IsKeyAutoGenerated property in view model based on which radio button is checked
            var radioButton = (RadioButton)sender;
            var viewModel = (EncryptTabViewModel)DataContext;
            viewModel.OperationChoice = radioButton.Name == "encryptRadioButton" ? DataTypes.OperationChoice.ENCRYPTION : DataTypes.OperationChoice.DECRYPTION;
        }
    }
}
