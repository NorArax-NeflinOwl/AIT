using AITLib.Helpers;
using AITLib.Models;

namespace AITLib.Databases
{
    public partial class MainContext
    {
        private static readonly object m_Locker = new object();
        private PrivateContext context;
        private static readonly MainContext instance = new MainContext();

        private MainContext()
        {
            context = new PrivateContext();
        }

        public static MainContext Instance
        {
            get
            {
                lock (m_Locker)
                {
                    return instance;
                }
            }
        }

        public User CreateSimpleUser(string login, string password)
        {
            var passwordHashed = HashGenerator.GenerateMD5Hash(password);
            return new User(login, passwordHashed);
        }

        public void CreateUserWithInfo(string login, string password, string firstName, string middleName, string lastName, int? pesel, DateTime? birthDate, string email, string phone, string nick)
        {
            var user = CreateSimpleUser(login, password);
            var userInfo = new UserInfo(user, firstName, middleName, lastName, pesel, birthDate, email, phone, nick);
            user.SetUserInfo(userInfo);

            AddUser(user);
        }

        public async void AddUser(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        public async void UpdateUser(uint userID, User user)
        {
            var item = context.Users.Where(q => q.ID == userID).FirstOrDefault();
            item = user;
            item.SetID(userID);
            await context.SaveChangesAsync();
        }

        public void RemoveUser(uint userID)
        {
            var item = context.Users.Where(q => q.ID == userID).FirstOrDefault();
            RemoveUser(item);
        }

        public async void RemoveUser(User user)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }

        public User FindUser(uint userID)
        {
            return context.Users.Where(q => q.ID == userID).FirstOrDefault();
        }

        public Note FindNote(uint noteID)
        {
            return context.Notes.Where(q => q.ID == noteID).FirstOrDefault();
        }

        public async void CreateNote(User user, string title, string description)
        {
            var note = new Note(user, title, description);
            user.AddNote(note);
            await context.SaveChangesAsync();
        }

        public void UpdateNote(uint noteID, string title, string description)
        {
            UpdateNote(FindNote(noteID), title, description);
        }

        public async void UpdateNote(Note note, string title, string description)
        {
            note.Update(title, description);
            await context.SaveChangesAsync();
        }

        public void RemoveNote(User user, uint noteID)
        {
            RemoveNote(user, FindNote(noteID));
        }

        public async void RemoveNote(User user, Note note)
        {
            user.RemoveNote(note);
            await context.SaveChangesAsync();
        }
    }
}
