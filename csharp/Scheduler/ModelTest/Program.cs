using AITLib.Databases;

string login = "admin";
string password = "admin";

var user = MainContext.Instance.CreateSimpleUser(login, password);

Console.WriteLine("Login: " + user.Login + "\nPasswordHashed: " + user.PasswordHash);

MainContext.Instance.AddUser(user);

user = MainContext.Instance.FindUser(user.ID);
Console.WriteLine("Login: " + user.Login + "\nPasswordHashed: " + user.PasswordHash);