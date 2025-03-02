namespace AppSearch.MVC.Models
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class AppModel
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public string Name { get; private set; }
        public string? FullVersion => WebServiceVersion?.FullVersion;
        public string? Version => WebServiceVersion?.Version;
        public string? Revision => WebServiceVersion?.Revision.ToString();
        public string? TargetPath { get; private set; }
        public string? WebServiceUrl { get; private set; }
        public WebServiceVersionModel? WebServiceVersion { get; private set; }
        public string ShortName { get; private set; }

        public AppModel(string name, string? targetPath)
        {
            Name = name;
            TargetPath = targetPath;
        } 

        public void SetShortName(string shortName)
        {
            ShortName = shortName;
        }

        public void GenerateAppVersion(ConfigurationModel config, string? websiteoutPut)
        {
            if (!string.IsNullOrEmpty(websiteoutPut))
            {
                WebServiceVersion = new WebServiceVersionModel(config, websiteoutPut);
            }
        }

        public void GenerateAppVersion(string? version)
        {
            if(!string.IsNullOrEmpty(version))
            {
                WebServiceVersion = new WebServiceVersionModel(version);
            }
        }

        public void SetWebServiceUrl(string gcmWebService)
        {
            WebServiceUrl = gcmWebService.Replace("Gcm", ShortName);
        }

        public string GetWebServiceName()
        {
            string webserviceName = string.Empty;
            string gcmPart = "gcm/";
            if (!string.IsNullOrEmpty(WebServiceUrl) && WebServiceUrl.Contains(gcmPart))
            {
                webserviceName = WebServiceUrl.Substring(WebServiceUrl.IndexOf(gcmPart) + gcmPart.Length)
                    .Substring(0, (ShortName + "WebServices").Length);
            }
            return webserviceName;
        }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(Revision))
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
