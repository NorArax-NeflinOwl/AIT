using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace AIT.DataBases.DBModel
{
    [Table("AitPersonsDetail")]
    public class AitPersonsDetail : ISerializable, ICloneable
    {
        [Key, ForeignKey("Person"), Column("prdPersonID", Order = 0)]
        public int PersonID { get; set; }
        [Column("prdFName", Order = 1)]
        public string FName { get; set; }
        [Column("prdLName", Order = 2)]
        public string LName { get; set; }
        [Column("prdBDate", Order = 3)]
        public DateTime BDate { get; set; }
        [MaxLength(11), Column("prdPesel", Order = 4)]
        public string Pesel { get; set; }
        public virtual AitPerson Person { get; set; }

        public AitPersonsDetail() { }

        public AitPersonsDetail(SerializationInfo info, StreamingContext context)
        {
            PersonID = (int)info.GetValue("PersonID", typeof(int));
            FName = (string)info.GetValue("FName", typeof(string));
            LName = (string)info.GetValue("LName", typeof(string));
            BDate = (DateTime)info.GetValue("BDate", typeof(DateTime));
            Pesel = (string)info.GetValue("Pesel", typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("PersonID", PersonID, typeof(int));
            info.AddValue("FName", FName, typeof(string));
            info.AddValue("LName", LName, typeof(string));
            info.AddValue("BDate", BDate, typeof(DateTime));
            info.AddValue("Pesel", Pesel, typeof(string));
        }

        public object Clone()
        {
            var detail = new AitPersonsDetail();
            detail.PersonID = PersonID;
            detail.FName = FName;
            detail.LName = LName;
            detail.BDate = BDate;
            detail.Pesel = Pesel;
            detail.Person = Person;
            return detail;
        }
    }
}
