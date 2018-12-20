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

    public class FormatDocumentConfiguration : IConfiguration
    {
        private readonly IConfiguration configuration;

        public FormatDocumentConfiguration(Document doc, IConfiguration defaultCfg)
        {
            configuration = defaultCfg;

            FileInfo cfgFile = new FileInfo(Path.Combine(doc.Path, ".formatconfig"));
            var dir = new DirectoryInfo(doc.Path);
            while (dir.Parent != null)
            {
                if (cfgFile.Exists) break;

                var configs = dir.GetFiles(".formatconfig");
                if (configs.Length > 0)
                {
                    cfgFile = configs[0];
                    break;
                }
                else
                {
                    dir = dir.Parent;
                }
            }

            if (cfgFile.Exists)
                configuration = new EditorConfigConfiguration(cfgFile.FullName);
        }

        public IEnumerable<string> Allowed => configuration.Allowed;

        public IEnumerable<string> Denied => configuration.Denied;

        public string Command => configuration.Command;
    }

    public interface IConfiguration
    {
        /// <summary>
        /// Allowed extensions. For example: .cs .html .cshtml .vb
        /// </summary>
        IEnumerable<string> Allowed { get; }

        /// <summary>
        /// Denied filed extentions. For example: .cs .html .cshtml .vb
        /// </summary>
        IEnumerable<string> Denied { get; }

        /// <summary>
        /// The Visual Studio command to execute. Defaults to format document (Edit.FormatDocument)
        /// </summary>
        string Command { get; }
    }

    public class EditorConfigConfiguration : IConfiguration
    {
        string allowed = ".*";
        string denied = "";
        string command = "Edit.FormatDocument";

        public EditorConfigConfiguration(string formatConfigFile)
        {
            var parser = new EditorConfig.Core.EditorConfigParser(formatConfigFile);
            FileConfiguration configFile = parser.Parse(formatConfigFile).First();

            if (configFile.Properties.ContainsKey("allowed_extensions"))
                configFile.Properties.TryGetValue("allowed_extensions", out allowed);

            if (configFile.Properties.ContainsKey("denied_extensions"))
                configFile.Properties.TryGetValue("denied_extensions", out denied);

            if (configFile.Properties.ContainsKey("command"))
                configFile.Properties.TryGetValue("command", out command);
        }

        IEnumerable<string> IConfiguration.Allowed
        {
            get
            {
                return allowed.Split(' ');
            }
        }

        IEnumerable<string> IConfiguration.Denied
        {
            get
            {
                return denied.Split(' ');
            }
        }

        public string Command => command;
    }

    public class VisualStudioConfiguration : DialogPage, IConfiguration
    {
        string allowed = ".*";
        string denied = "";
        string command = "Edit.FormatDocument";

        [Category("Format Document On Save")]
        [DisplayName("Allowed extensions")]
        [Description("Space separated list. For example: [.cs .html .cshtml .vb] Overrides extensions listed in Denied section. When you use [.*] the extensions listed in Denied section will be ignored. Empty value respects all extensions listed in Denied section.")]
        public string Allowed
        {
            get { return allowed; }
            set { allowed = value; }
        }

        [Category("Format Document On Save")]
        [DisplayName("Denied extensions")]
        [Description("Space separated list. For example: [.cs .html .cshtml .vb]")]
        public string Denied
        {
            get { return denied; }
            set { denied = value; }
        }

        [Category("Format Document On Save")]
        [DisplayName("Command")]
        [Description("The Visual Studio command to execute. Defaults to VS command [Edit.FormatDocument]")]
        public string Command
        {
            get { return command; }
            set { command = value; }
        }

        IEnumerable<string> IConfiguration.Allowed
        {
            get
            {
                return Allowed.Split(' ');
            }
        }

        IEnumerable<string> IConfiguration.Denied
        {
            get
            {
                return Denied.Split(' ');
            }
        }
    }
}
