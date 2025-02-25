namespace AppSearch.MVC.Models
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class EnviromentModel
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public bool? IsActive { get; private set; }
        public string EnvName { get; private set; }
        public string ClientName { get; private set; }
        public string System { get; private set; }
        public AppModel AppModel { get; private set; }
        public string? WebServiceUrl { get; private set; }
        public WebServiceVersionModel? GcmWebServiceVersion { get; private set; }

        public EnviromentModel(ConfigurationModel config, string name, string client, AppModel model, bool? isActive)
        {
            EnvName = name;
            AppModel = model;
            ClientName = client;
            System = GetSystemFromEnvName(config);
            IsActive = isActive;
            WebServiceUrl = GetGcmWebServiceUrl(config, false, System, name, config.DefaulPort);
            model.SetWebServiceUrl(WebServiceUrl);
        }

        public void UpdateActive(bool? isActive)
        {
            IsActive = isActive;
        }

        public void UpdateTargetUri(string targetUri)
        {
            WebServiceUrl = targetUri;
        }

        public void GenerateGcmVersion(ConfigurationModel config, string? websiteoutPut)
        {
            if (!string.IsNullOrEmpty(websiteoutPut))
            {
                GcmWebServiceVersion = new WebServiceVersionModel(config, websiteoutPut);
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is EnviromentModel model
                && IsActive == model.IsActive
                && EnvName == model.EnvName
                && ClientName == model.ClientName
                && AppModel.Equals(model.AppModel);
        }

        public override string ToString()
        {
            return string.Format("{0} | {1}", EnvName, ClientName);
        }

        public static string GetGcmWebServiceUrl(ConfigurationModel config, 
            bool hasHttps, string? system, string envNo, int port)
        {
            return string.Format(Properties.Resources.DefaultGcmWebServicesUrl,
                hasHttps ? "s" : string.Empty,
                system ?? string.Empty,
                envNo.Equals(Properties.Resources.LocalEnvName) ? envNo 
                    : envNo.Remove(0, 1), 
                port);
        }

        private string GetSystemFromEnvName(ConfigurationModel config)
        {
            if (EnvName.StartsWith('Q'))
                return config.EnvPrefix.WindowsPrefix;
            if (EnvName.StartsWith('L'))
                return config.EnvPrefix.LinuxPrefix;
            return string.Empty;
        }
    }
}
