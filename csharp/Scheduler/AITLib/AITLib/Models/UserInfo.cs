using AITLib.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace AITLib.Models
{
    public class UserInfo : BaseObject
    {
        private uint id;
        private User parent;
        private DateTime? updatedDate;
        private string firstName;
        private string middleName;
        private string lastName;
        private int? pesel;
        private DateTime? birthDate;
        private string email;
        private string phone;
        private string nick;

        public uint ID { get => id; }
        [Key, ForeignKey("User"), Column("usiParentID", Order = 0)]
        public uint ParentID { get => parent.ID; }
        [Column("usiUpdateDate", Order = 1)]
        public DateTime UpdatedDate { get => updatedDate ?? parent.CreatedDate; }
        [Column("usiFirstName", Order = 2)]
        public string FirstName { get => firstName; }
        [Column("usiMiddleName", Order = 3)]
        public string MiddleName { get => middleName; }
        [Column("usiLastName", Order = 4)]
        public string LastName { get => lastName; }
        [MaxLength(11), Column("usiPesel", Order = 5)]
        public int? Pesel { get => pesel; }
        [Column("usiBirthDate", Order = 6)]
        public DateTime? BirthDate { get => birthDate; }
        [Column("usiEmail", Order = 7)]
        public string Email { get => email; }
        [Column("usiPhone", Order = 8)]
        public string Phone { get => phone; }
        [Column("usiNick", Order = 9)]
        public string Nick { get => nick; }

        private UserInfo()
        {
        }

        public UserInfo(User parent)
        {
            this.parent = parent;
        }

        public UserInfo(User user, string firstName, string middleName, string lastName, int? pesel, DateTime? birthDate, string email, string phone, string nick) : this(user)
        {
            this.firstName = firstName;
            this.middleName = middleName;
            this.lastName = lastName;
            this.pesel = pesel;
            this.birthDate = birthDate;
            this.email = email;
            this.phone = phone;
            this.nick = nick;
        }

        public UserInfo(SerializationInfo info, StreamingContext context)
        {
            id = (uint)info.GetValue("ID", typeof(uint));
            parent = (User)info.GetValue("Parent", typeof(User));
            updatedDate = (DateTime)info.GetValue("UpdatedDate", typeof(DateTime));
            firstName = (string)info.GetValue("FirstName", typeof(string));
            middleName = (string)info.GetValue("MiddleName", typeof(string)); ;
            lastName = (string)info.GetValue("LastName", typeof(string));
            pesel = (int?)info.GetValue("Pesel", typeof(int?));
            birthDate = (DateTime?)info.GetValue("BirthDate", typeof(DateTime?));
            email = (string)info.GetValue("Email", typeof(string));
            phone = (string)info.GetValue("Phone", typeof(string));
            nick = (string)info.GetValue("Nick", typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID", id, typeof(uint));
            info.AddValue("Parent", parent, typeof(User));
            info.AddValue("UpdatedDate", updatedDate, typeof(DateTime));
            info.AddValue("FirstName", firstName, typeof(string));
            info.AddValue("MiddleName", middleName, typeof(string));
            info.AddValue("LastName", lastName, typeof(string));
            info.AddValue("Pesel", pesel, typeof(int?));
            info.AddValue("BirthDate", birthDate, typeof(DateTime?));
            info.AddValue("Email", email, typeof(string));
            info.AddValue("Phone", phone, typeof(string));
            info.AddValue("Nick", nick, typeof(string));
        }

        public object Clone()
        {
            var userInfo = new UserInfo();
            userInfo.id = id;
            userInfo.parent = parent;
            userInfo.updatedDate = updatedDate;
            userInfo.firstName = firstName;
            userInfo.middleName = middleName;
            userInfo.lastName = lastName;
            userInfo.pesel = pesel;
            userInfo.birthDate = birthDate;
            userInfo.email = email;
            userInfo.phone = phone;
            userInfo.nick = nick;
            return userInfo;
        }

    }
}
