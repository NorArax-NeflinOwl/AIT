namespace WPF.Models.Interfaces
{
    public interface ISerializableForSession
    {
        void SerializeSession();
        void DeserializaSession();
    }
}
