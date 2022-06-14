// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NuGet.ProjectManagement;

namespace NuGet.Test.Utility
{
    public class CentralPackageVersionsManagementFile
    {
        private const string DirectoryPackagesProps = "Directory.Packages.props";

        private FileInfo _path;

        private Dictionary<string, string> _packageVersions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, string> _centralPackageReferences = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private bool _managePackageVersionsCentrally;

        private CentralPackageVersionsManagementFile(string path, bool managePackageVersionsCentrally)
        {
            _path = new FileInfo(Path.Combine(path, DirectoryPackagesProps));
            _managePackageVersionsCentrally = managePackageVersionsCentrally;
        }

        public static CentralPackageVersionsManagementFile Create(string path, bool managePackageVersionsCentrally = true)
        {
            return new CentralPackageVersionsManagementFile(path, managePackageVersionsCentrally);
        }


        public CentralPackageVersionsManagementFile SetCentralPackageReference(string packageId, string packageVersion)
        {
            _centralPackageReferences[packageId] = packageVersion;
            return this;
        }

        public CentralPackageVersionsManagementFile SetPackageVersion(string packageId, string packageVersion)
        {
            _packageVersions[packageId] = packageVersion;
            return this;
        }

        public CentralPackageVersionsManagementFile RemovePackageVersion(string packageId)
        {
            _packageVersions.Remove(packageId);
            return this;
        }

        public void Save()
        {
            XDocument cpvm = new XDocument(
                new XElement("Project",
                    new XElement("PropertyGroup",
                        new XElement(ProjectBuildProperties.ManagePackageVersionsCentrally, new XText(_managePackageVersionsCentrally.ToString()))),
                    new XElement("ItemGroup", _packageVersions.Select(i => new XElement("PackageVersion", new XAttribute("Include", i.Key), new XAttribute("Version", i.Value)))),
                    new XElement("ItemGroup", _centralPackageReferences.Select(i => new XElement("CentralPackageReference", new XAttribute("Include", i.Key), new XAttribute("Version", i.Value))))));

            cpvm.Save(_path.FullName);
        }
    }
}
