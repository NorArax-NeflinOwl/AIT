using System;
using System.Runtime.Serialization;
using WPF.Databases.Contexts;
using WPF.Enums;

namespace WPF.Models
{
    public class LogInfoModel : ISerializable, ICloneable
    {
        public FileTypeEnum Type { get; set; }
        public DateTime Date { get; set; }
        public DeviceInfoModel DeviceInfo { get; set; }
        public string AccountID { get; set; }
        public string Message { get; set; }

        public LogInfoModel()
        {
            Date = DateTime.Now;
            AccountID = PDBContext.Instance.AccountID;
            DeviceInfo = PDBContext.Instance.BuildDeviceInfo();
        }

        public LogInfoModel(SerializationInfo info, StreamingContext context)
        {
            Type = (FileTypeEnum)info.GetValue(nameof(Type), typeof(FileTypeEnum));
            Date = (DateTime)info.GetValue(nameof(Date), typeof(DateTime));
            DeviceInfo = (DeviceInfoModel)info.GetValue(nameof(DeviceInfo), typeof(DeviceInfoModel));
            AccountID = (string)info.GetValue(nameof(AccountID), typeof(string));
            Message = (string)info.GetValue(nameof(Message), typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Type), Type, typeof(FileTypeEnum));
            info.AddValue(nameof(Date), Date, typeof(DateTime));
            info.AddValue(nameof(DeviceInfo), DeviceInfo, typeof(DeviceInfoModel));
            info.AddValue(nameof(AccountID), AccountID, typeof(string));
            info.AddValue(nameof(Message), Message, typeof(string));
        }

        public object Clone()
        {
            var clone = new LogInfoModel
            {
                Type = Type,
                Date = Date,
                DeviceInfo = (DeviceInfoModel)DeviceInfo.Clone(),
                AccountID = AccountID,
                Message = Message
            };
            return clone;
        }
    }
}
