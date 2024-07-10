using System;
using WixSharp;

namespace Diario_Setup
{
    internal class Program
    {
        static void Main()
        {
            var project = new Project("Diario",
                              new Dir(@"[ProgramFiles64Folder]\\Diario",
                                  new DirFiles(@"*.*"),
                                  new Dir("runtimes",
                                      new Dir("win-x64",
                                            new Dir("native",
                                                new File("runtimes\\win-x64\\native\\av_libglesv2.dll"),
                                                new File("runtimes\\win-x64\\native\\libHarfBuzzSharp.dll"),
                                                new File("runtimes\\win-x64\\native\\libSkiaSharp.dll"),
                                                new File("runtimes\\win-x64\\native\\e_sqlite3.dll")
                                    )
                                 )
                              )
                        ),
                        new Dir(@"%ProgramMenu%",
                         new ExeFileShortcut("Diario", "[ProgramFiles64Folder]\\Diario\\Diario.avalonia.Desktop.exe", "") { WorkingDirectory = "[INSTALLDIR]" }
                      )//,
                       //new Property("ALLUSERS","0")
            );

            project.GUID = new Guid("47F5EFC4-E7EF-4484-87AE-0CB4BA3BFD30");
            project.Version = new Version("0.3");
            project.Platform = Platform.x64;
            project.SourceBaseDir = "E:\\source\\Diario.Avalonia\\Diario.avalonia.Desktop\\bin\\Release\\net8.0-windows10.0.22621.0";
            project.LicenceFile = "LICENSE.rtf";
            project.OutDir = "E:\\";
            project.ControlPanelInfo.Manufacturer = "Giulio Sorrentino";
            project.ControlPanelInfo.Name = "Diario";
            project.ControlPanelInfo.HelpLink = "https://github.com/numerunix/Diario.Avalonia/issues";
            project.Description = "Un diario privato in avalonia e linq";
            //            project.Properties.SetValue("ALLUSERS", 0);
            project.BuildMsi();
        }
    }
}