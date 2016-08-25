using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;

namespace Laktak.Hjson
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
  // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
  // a package.
  [PackageRegistration(UseManagedResourcesOnly = true)]
  // This attribute is used to register the information needed to show this package
  // in the Help/About dialog of Visual Studio.
  [ProvideAutoLoad(UIContextGuids80.NoSolution)]
  [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
  [Guid(HjsonPkgString)]
  public sealed class HjsonPackage : AsyncPackage
  {
    public const string HjsonPkgString="b8897eb0-2b15-4d18-89e2-79ec54cd2c86";

    protected override async System.Threading.Tasks.Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
    {
      await copySyntax();
    }

    static async System.Threading.Tasks.Task copySyntax()
    {
      string src=Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data");
      string ext=Environment.ExpandEnvironmentVariables("%userprofile%\\.vs\\Extensions");
      string target=Path.Combine(ext, "Hjson");

      await System.Threading.Tasks.Task.Run(() =>
      {
        try
        {
          if (!Directory.Exists(target))
            Directory.CreateDirectory(target);

          File.Copy(
            Path.Combine(src, "Hjson.tmLanguage"),
            Path.Combine(target, "Hjson.tmLanguage"),
            true);
        }
        catch (Exception e)
        {
          System.Diagnostics.Debug.Write(e);
        }
      });
    }  
  }
}