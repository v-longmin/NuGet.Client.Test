// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
=======
using System.Windows.Controls;
>>>>>>> 2ac31786cc92b0d57478166005fe0a87a064103d

namespace NuGet.Options
{
    /// <summary>
    /// Interaction logic for PackageSourceMappingOptionsControl.xaml
    /// </summary>
    public partial class PackageSourceMappingOptionsControl : UserControl
    {
<<<<<<< HEAD
        public ICommand ShowButtonCommand { get; set; }

        public ICommand HideButtonCommand { get; set; }

        public ICommand AddButtonCommand { get; set; }

        // public ICommand IsCheckedCommand { get; set; }

        public ICommand RemoveButtonCommand { get; set; }

        public ICommand ClearButtonCommand { get; set; }


        public PackageSourceMappingOptionsControl()
        {
            ShowButtonCommand = new ShowButtonCommand(ExecuteShowButtonCommand, CanExecuteShowButtonCommand);

            HideButtonCommand = new HideButtonCommand(ExecuteHideButtonCommand, CanExecuteHideButtonCommand);

            AddButtonCommand = new AddButtonCommand(ExecuteAddButtonCommand, CanExecuteAddButtonCommand);

            RemoveButtonCommand = new RemoveButtonCommand(ExecuteRemoveButtonCommand, CanExecuteRemoveButtonCommand);

            ClearButtonCommand = new ClearButtonCommand(ExecuteClearButtonCommand, CanExecuteClearButtonCommand);

            DataContext = this;

            InitializeComponent();

            (ShowButtonCommand as ShowButtonCommand).InvokeCanExecuteChanged();
        }




        private void ExecuteShowButtonCommand(object parameter)
        {
            MyPopup.IsOpen = true;
        }

        private bool CanExecuteShowButtonCommand(object parameter)
        {
            if (MyPopup != null && MyPopup.IsOpen == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ExecuteHideButtonCommand(object parameter)
        {
            MyPopup.IsOpen = false;
            (ShowButtonCommand as ShowButtonCommand).InvokeCanExecuteChanged();
        }

        private bool CanExecuteHideButtonCommand(object parameter)
        {
            return true;
        }


        private void ExecuteAddButtonCommand(object parameter)
        {
            MyPopup.IsOpen = false;
            var tempPkgID = packageID.Text;
            var temp = sourcesListBox.SelectedItems;

            PackageItem tempPkg = new PackageItem(tempPkgID, temp);
            packageList.Items.Add(tempPkg);
            (ShowButtonCommand as ShowButtonCommand).InvokeCanExecuteChanged();
            (RemoveButtonCommand as RemoveButtonCommand).InvokeCanExecuteChanged();
            (ClearButtonCommand as ClearButtonCommand).InvokeCanExecuteChanged();
        }

        private bool CanExecuteAddButtonCommand(object parameter)
        {
            var tempPkg = packageID.Text;
            return !string.IsNullOrWhiteSpace(tempPkg);
        }

        private void ExecuteRemoveButtonCommand(object parameter)
        {
            packageList.Items.Remove(packageList.SelectedItem);
            (ClearButtonCommand as ClearButtonCommand).InvokeCanExecuteChanged();
        }

        private bool CanExecuteRemoveButtonCommand(object parameter)
        {
            return MyPopup != null && packageList.Items.Count > 0;
        }

        private void ExecuteClearButtonCommand(object parameter)
        {
            packageList.Items.Clear();
            (RemoveButtonCommand as RemoveButtonCommand).InvokeCanExecuteChanged();
        }

        private bool CanExecuteClearButtonCommand(object parameter)
        {
            return MyPopup != null && packageList.Items.Count > 0;
        }

=======
        public PackageSourceMappingOptionsControl()
        {
            InitializeComponent();
        }
>>>>>>> 2ac31786cc92b0d57478166005fe0a87a064103d
    }
}
