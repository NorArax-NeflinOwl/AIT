using WPF.Models.Extensions;
using WPF.Models.Interfaces;

namespace WPF.UI.ViewModels
{
    public class LoginViewModel : NotifyPropertyChangedExtension
    {
        private IWindowsProperties properties;
        public string Login
        {
            get { return properties.Properties.ContainsKey(nameof(Login)) ? properties.Properties[nameof(Login)].ToString() : string.Empty; }
        }

        public LoginViewModel(IWindowsProperties properties) : base(null)
        {
            this.properties = properties;
        }
    }
}
