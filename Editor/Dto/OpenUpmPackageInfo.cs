using System.Collections.Generic;

namespace Achieve.Package.Manager
{
    public class OpenUpmPackageInfo
    {
        public string Name;
        public string DisplayName;
        public string Description;
        public string LatestVersion;
        public List<string> Versions = new List<string>();
    }
}
