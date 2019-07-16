using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.Serialization;
using AIT.Interface;

namespace AIT.Interfaces
{
    public interface aitDataBaseInterface : ICloneable, ISerializable
    {
        int ID { get; }
        string TabName { get; }
        DateTime CreateDate { get; }
        DateTime? UpdateDate { get; }
        
        void SetTabName(string name);
        void SetID(int newId, bool isCloning = false);
        void Update();
    }
}
