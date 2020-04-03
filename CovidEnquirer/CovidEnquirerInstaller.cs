using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;

namespace CovidEnquirer
{
    [RunInstaller(true)]
    public partial class CovidEnquirerInstaller : Installer
    {
        public CovidEnquirerInstaller()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary savedState)
        {
            base.Install(savedState);
        }

        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }

        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);

            FileInfo info = new FileInfo(this.Context.Parameters["AssemblyPath"]);
            if (info.Exists)
            {
                string workingDirectory = info.DirectoryName;
                ProcessStartInfo start = new ProcessStartInfo(info.FullName)
                {
                    WorkingDirectory = workingDirectory,
                };

                Process.Start(start);
            }
        }
    }
}
