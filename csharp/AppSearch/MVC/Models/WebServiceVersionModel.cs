namespace AppSearch.MVC.Models
{
    public class WebServiceVersionModel
    {
        public string FullVersion;
        public string Version;
        public uint? Revision;

        public WebServiceVersionModel(ConfigurationModel config, string websiteOutput)
        {
            FullVersion = GetFullVersion(config,websiteOutput);
            Version = GetVersion(config, websiteOutput);
            Revision = GetRevision(config, websiteOutput);
        }

        private string GetFullVersion(ConfigurationModel config, string input)
        {
            string version = input.Substring(input.IndexOf(config.WebServiceInfo.FullVerBegin) + config.WebServiceInfo.FullVerBegin.Length);
            if(version.Contains(config.WebServiceInfo.FullVerEnd))
                return version.Remove(version.IndexOf(config.WebServiceInfo.FullVerEnd));
            else if(version.Contains(config.WebServiceInfo.AppFullVerEnd))
                return version.Remove(version.IndexOf(config.WebServiceInfo.AppFullVerEnd));
            else
                return version.Remove(version.IndexOf(config.WebServiceInfo.CurVerEnd));
        }

        private string GetVersion(ConfigurationModel config, string input)
        {
            if (input.Contains(config.WebServiceInfo.HFstamp))
            {
                string text9 = input.Substring(input.IndexOf(config.WebServiceInfo.FullVerBegin) + config.WebServiceInfo.FullVerBegin.Length);
                string text8 = text9.Remove(text9.IndexOf(config.WebServiceInfo.HFstamp));
                return (text8.Contains('-') ? text8.Remove(text8.IndexOf('-')) : text8);
            }
            else
            {
                string version = input.Substring(input.IndexOf(config.WebServiceInfo.CurVerBegin) + config.WebServiceInfo.CurVerBegin.Length);
                return version.Remove(version.IndexOf(config.WebServiceInfo.CurVerEnd));
            }
        }

        private uint? GetRevision(ConfigurationModel config, string input)
        {
            uint? rev = null;
            if(input.Contains(config.WebServiceInfo.RevBegin))
            {
                string revision = input.Substring(input.IndexOf(config.WebServiceInfo.RevBegin) + config.WebServiceInfo.RevBegin.Length);
                string result = revision.Remove(revision.IndexOf(config.WebServiceInfo.RevEnd));
                if (Int32.TryParse(result, out int integer))
                {
                    rev = (uint?)integer;
                }
            }
            return rev;
        }

        public override string ToString()
        {
            return string.Format("{0} \n{1} \n{2}", FullVersion, Version, Revision);
        }
    }
}



/*
3.5.73.0.67-2024.04.15_07.28
3.5.73.0.67-2024.04.15_07.28 <!-- HF_nr -->

3.5.73.0.67-2024.04.15_07.28 -HF2024_04_15__10_04.1
3.5.73.0.67
653724

http://wp373.softsystem.pl:7700/gcm/GcmWebServices/
http://wp373.softsystem.pl:7700/gcm/SoftFlwWebServices/


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

















<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head>
	<title>Gcm WebServices - Administration</title>
	<meta http-equiv="content-type" content="text/html; charset=UTF-8"/>
	<meta http-equiv="pragma" content="no-cache" />
	
		
		
			<script type="text/javascript" src="jquery/jquery-1.6.2.min.js"></script>
		
	
	<style type="text/css">@import url(css/style.css);</style>

    


</head>
<body>
<div id="header">
	<p class="title">Gcm</p>
	<p>Environment: <b>Q373</b>
	<br />Full Version: <b>3.5.73.0.67-2024.04.15_07.28 -HF2024_04_15__10_04.1-UNSIGNED</b>
	
	<br />SVN Revision: <b>653724</b> SVN Path: <b>gene/hotfixes/3_5_73_0_HF</b>
	<br />Host Name: <i>wp373.softsystem.pl</i> 
	<br />Server Details: <i>gcm_0000</i>
	
	
	</p>
</div>
<div id="menu">

  <a href="?a=login" class="current lastMenuItem" >Login</a>

</div>
<div id="content">

	<p>Please login.</p>
	<div id="loginContent">
		<form name="loginForm" id="loginForm" method="post" action="index.jsp">
			<input type="hidden" name="a" value=""/>
			<label for="login">Login:</label>
			<input name="login" style="width: 150px;" type="text" value=""/><br /><br />
			<label for="pass">Password:</label>
			<input name="pass" style="width: 150px;" type="password" /><br /><br />
			<input type="submit" name="submitLogin" value="Login"/>
		</form>
	</div>
	<script type="text/javascript">
		document.loginForm.login.focus();
		document.loginForm.login.select();
	</script>
</div>
</body>
</html>
*/