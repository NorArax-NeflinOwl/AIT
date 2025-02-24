namespace AppSearch.MVC.Models
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class AppModel
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public string Name { get; private set; }
        public string? FullVersion => WebServiceVersion?.FullVersion;
        public string? Version => WebServiceVersion?.Version;
        public uint? Revision => WebServiceVersion?.Revision;
        public string? TargetPath { get; private set; }
        public WebServiceVersionModel? WebServiceVersion { get; private set; }

        public AppModel(string name, string? targetPath)
        {
            Name = name;
            TargetPath = targetPath;
        }

        public void GenerateAppVersion(ConfigurationModel config, string websiteoutPut)
        {
            if (!string.IsNullOrEmpty(websiteoutPut))
            {
                WebServiceVersion = new WebServiceVersionModel(config, websiteoutPut);
            }
        }

        public override string ToString()
        {
            if (Revision != null)
            {
                return string.Format("{0} | Version: {1} | Rev: {2}", Name, Version, Revision);
            }
            if(!string.IsNullOrEmpty(Version))
            {
                return string.Format("{0} | Version: {1}", Name, Version);
            }
            return Name;
        }

        public override bool Equals(object? obj)
        {
            return obj is AppModel model
                && Name == model.Name
                && Version == model.Version
                && Revision == model.Revision
                && TargetPath == model.TargetPath;
        }
    }

}
