using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EditorConfig.Core;
using System;
using Microsoft.VisualStudio;
using Elders.VSE_FormatDocumentOnSave.Configurations;

namespace Elders.VSE_FormatDocumentOnSave
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true)]   // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is a package.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // This attribute is used to register the information needed to show this package in the Help/About dialog of Visual Studio.

    //Set the UI context to autoload a VSPackage.																				
    //Needs to autoload in multiple scenarios to support multiple Visual Studio configurations (ex. Folder View). 
    [ProvideAutoLoad(UIContextGuids80.NoSolution)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [ProvideAutoLoad(UIContextGuids80.SolutionHasSingleProject)]
    [ProvideAutoLoad(UIContextGuids80.SolutionHasMultipleProjects)]

    [Guid(GuidList.guidVSPackage2PkgString)]
    [ProvideOptionPage(typeof(VisualStudioConfiguration), "Format Document On Save", "General", 0, 0, true)]
    public sealed class FormatDocumentOnSavePackage : Package, IAsyncLoadablePackageInitialize
    {
        private bool isAsyncLoadSupported;
        private FormatDocumentOnBeforeSave plugin;

        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public FormatDocumentOnSavePackage() { }

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            isAsyncLoadSupported = this.IsAsyncPackageSupported();

            // Only perform initialization if async package framework is not supported
            if (!isAsyncLoadSupported)
            {
                var dte = (DTE)GetService(typeof(DTE));

                var runningDocumentTable = new RunningDocumentTable(this);
                var defaultConfig = (VisualStudioConfiguration)GetDialogPage(typeof(VisualStudioConfiguration));

                var documentFormatService = new DocumentFormatService(dte, (doc) => new FormatDocumentConfiguration(doc, defaultConfig));
                plugin = new FormatDocumentOnBeforeSave(dte, runningDocumentTable, documentFormatService);
                runningDocumentTable.Advise(plugin);
            }

            base.Initialize();
        }

        public IVsTask Initialize(IAsyncServiceProvider pServiceProvider, IProfferAsyncService pProfferService, IAsyncProgressCallback pProgressCallback)
        {
            if (!isAsyncLoadSupported)
            {
                throw new InvalidOperationException("Async Initialize method should not be called when async load is not supported.");
            }

            return ThreadHelper.JoinableTaskFactory.RunAsync<object>(async () =>
            {
                var dte = await pServiceProvider.GetServiceAsync<DTE>(typeof(DTE));

                var runningDocumentTable = new RunningDocumentTable(this);
                var defaultConfig = (VisualStudioConfiguration)GetDialogPage(typeof(VisualStudioConfiguration));

                var documentFormatService = new DocumentFormatService(dte, (doc) => new FormatDocumentConfiguration(doc, defaultConfig));
                plugin = new FormatDocumentOnBeforeSave(dte, runningDocumentTable, documentFormatService);
                runningDocumentTable.Advise(plugin);

                return null;
            }).AsVsTask();
        }
    }


}
