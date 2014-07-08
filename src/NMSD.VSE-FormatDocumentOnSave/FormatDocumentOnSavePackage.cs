using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;

namespace NMSD.VSE_FormatDocumentOnSave
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
    [ProvideAutoLoad("{f1536ef8-92ec-443c-9ed7-fdadf150da82}")] //To set the UI context to autoload a VSPackage
    [Guid(GuidList.guidVSPackage2PkgString)]
    public sealed class FormatDocumentOnSavePackage : Package
    {
        FormatDocumentOnBeforeSaveCommand plugin;

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
            DTE dte = (DTE)base.GetService(typeof(DTE));
            var txtMgr = (IVsTextManager)base.GetService(typeof(SVsTextManager));
            plugin = new FormatDocumentOnBeforeSaveCommand(dte, new DocumentFormatter(txtMgr, dte));
            base.Initialize();
        }
    }
}
