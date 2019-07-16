using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIT.Interfaces;
using AIT.Constant;
using System.Runtime.Serialization;

namespace AIT.Models
{
    public class aitExtDataBaseModel : aitDataBaseInterface
    {
        #region Properties

        private int m_ID;
        public int ID
        {
            get { return m_ID; }
        }

        private string m_TabName;
        public string TabName
        {
            get { return m_TabName; }
        }

        protected DateTime m_CreateDate;
        public DateTime CreateDate
        {
            get { return m_CreateDate; }
        }

        private DateTime? m_UpdateDate;
        public DateTime? UpdateDate
        {
            get { return m_UpdateDate; }
        }

        #endregion
        #region Constructors

        public aitExtDataBaseModel() 
        {
            m_CreateDate = DateTime.Now;
            m_ID = m_CreateDate.ToUniversalTime().GetHashCode();
        }

        public aitExtDataBaseModel(DateTime cdate)
        {
            m_CreateDate = cdate;
            m_ID = m_CreateDate.ToUniversalTime().GetHashCode();
        }

        public aitExtDataBaseModel(SerializationInfo info, StreamingContext context)
        {
            m_ID = (int)info.GetValue("ID", typeof(int));
            m_TabName = (string)info.GetValue("TabName", typeof(string));
            m_CreateDate = (DateTime)info.GetValue("CreateDate", typeof(DateTime));
            m_UpdateDate = (DateTime?)info.GetValue("UpdateDate", typeof(DateTime?));
        }

        #endregion
        #region Implementation interface methods

        public void SetTabName(string name)
        {
            if(string.IsNullOrEmpty(m_TabName))
                m_TabName = name;
        }

        public void SetID(int newId, bool isCloning = false)
        {
            m_ID = newId;
            if (!isCloning) Update();
        }

        public void Update()
        {
            m_UpdateDate = DateTime.Now;
        }

        public virtual object Clone()
        {
            var newObj = new aitExtDataBaseModel(m_CreateDate);
            newObj.SetID(m_ID, true);
            newObj.SetTabName(m_TabName);
            newObj.m_CreateDate = m_CreateDate;

            return newObj;
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID", m_ID, typeof(int));
            info.AddValue("TabName", m_TabName, typeof(string));
            info.AddValue("CreateDate", m_CreateDate, typeof(DateTime));
            info.AddValue("UpdateDate", m_UpdateDate, typeof(DateTime?));
        }

        #endregion
    }
}
