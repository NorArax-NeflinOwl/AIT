namespace AppSearch.MVC.Models
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class EnviromentModel
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public bool? IsActive { get; private set; }
        public string EnvName { get; private set; }
        public string ClientName { get; private set; }
        public AppModel AppModel { get; private set; }

        public EnviromentModel(string name, string client, AppModel model, bool? isActive = null)
        {
            EnvName = name;
            AppModel = model;
            ClientName = client;
            IsActive = isActive;
        }

        public void UpdateActive(bool? isActive)
        {
            IsActive = isActive;
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
    }
}
