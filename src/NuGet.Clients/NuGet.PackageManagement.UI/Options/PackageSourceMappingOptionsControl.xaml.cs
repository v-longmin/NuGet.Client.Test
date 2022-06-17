// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.VisualStudio;

using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using System.Threading.Tasks;

using Microsoft;
using Microsoft.ServiceHub.Framework;

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.Configuration;
using NuGet.PackageManagement.UI;
using NuGet.PackageManagement.VisualStudio;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Common;
using NuGet.VisualStudio.Internal.Contracts;
using NuGet.VisualStudio.Telemetry;
using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.IAsyncServiceProvider;
using Task = System.Threading.Tasks.Task;

namespace NuGet.Options
{
    /// <summary>
    /// Interaction logic for PackageSourceMappingOptionsControl.xaml
    /// </summary>
    public partial class PackageSourceMappingOptionsControl : UserControl
    {

        public ObservableCollection<PackageSourceContextInfo> SourcesCollection { get; private set; }

        private IReadOnlyList<PackageSourceContextInfo> _originalPackageSources;
#pragma warning disable ISB001 // Dispose of proxies, disposed in disposing event or in ClearSettings
        private INuGetSourcesService _nugetSourcesService;
#pragma warning restore ISB001 // Dispose of proxies, disposed in disposing event or in ClearSettings

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


            // SourcesCollection = new ObservableCollection<object>();
            DataContext = this;

            InitializeComponent();

            (ShowButtonCommand as ShowButtonCommand).InvokeCanExecuteChanged();
        }

        internal async Task InitializeOnActivatedAsync(CancellationToken cancellationToken)
        {
            //_nugetSourcesService?.Dispose();
            //_nugetSourcesService = null;

            IServiceBrokerProvider serviceBrokerProvider = await ServiceLocator.GetComponentModelServiceAsync<IServiceBrokerProvider>();
            IServiceBroker serviceBroker = await serviceBrokerProvider.GetAsync();

#pragma warning disable ISB001 // Dispose of proxies, disposed in disposing event or in ClearSettings
            _nugetSourcesService = await serviceBroker.GetProxyAsync<INuGetSourcesService>(
                    NuGetServices.SourceProviderService,
                    cancellationToken: cancellationToken);
#pragma warning restore ISB001 // Dispose of proxies, disposed in disposing event or in ClearSettings


            _originalPackageSources = await _nugetSourcesService.GetPackageSourcesAsync(cancellationToken);

            SourcesCollection = new ObservableCollection<PackageSourceContextInfo>(_originalPackageSources);

            //_nugetSourcesService?.Dispose();
            //_nugetSourcesService = null;
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


        private ObservableCollection<string> Items { get; set; }

        internal void ClearSettings()
        {
            // clear this flag so that we will set up the bindings again when the option page is activated next time
            _nugetSourcesService?.Dispose();
            _nugetSourcesService = null;
        }
    }
}
