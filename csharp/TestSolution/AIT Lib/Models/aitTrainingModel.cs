using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIT_Lib.Constant;
using System.Runtime.Serialization;

namespace AIT_Lib.Models
{
    public class aitTrainingModel : aitSimpleNoteModel, ICloneable, ISerializable
    {
        #region Properties

        private aitTrainingTypes m_TrainingType = aitTrainingTypes.UNSPECIFIED;
        public aitTrainingTypes TrainingType
        {
            get
            {
                return m_TrainingType;
            }
        }

        private aitTimeOfDay m_TimeOfDay = aitTimeOfDay.UNF;
        public aitTimeOfDay TimeOfDay
        {
            get
            {
                return m_TimeOfDay;
            }
        }

        private uint m_Series;
        public uint Series
        {
            get
            {
                return m_Series;
            }
        }

        private uint m_Repeat;
        public uint Repeat
        {
            get
            {
                return m_Repeat;
            }
        }

        private uint m_KM;
        public uint KM
        {
            get
            {
                return m_KM;
            }
        }

        private uint m_Time;
        public uint Time
        {
            get
            {
                return m_Time;
            }
        }

        private uint m_Steps;
        public uint Steps
        {
            get
            {
                return m_Steps;
            }
        }

        #endregion
        #region Constructors

        public aitTrainingModel()
        {
            SetTabName(aitTabsNames.aitTBCalTrainings);
        }

        public aitTrainingModel(
            string title, 
            string note,
            DateTime? noteDate,
            uint series = 0,
            uint repeat = 0,
            aitTrainingTypes type = aitTrainingTypes.UNSPECIFIED, 
            aitTimeOfDay time = aitTimeOfDay.UNF, 
            aitDateTimeType dateType = aitDateTimeType.DATE)
            : base(title, note, noteDate, dateType, aitNotesTypesModel.TRAINING_MODEL)
        {
            m_TimeOfDay = time;
            m_TimeOfDay = time;
            m_Series = series;
            m_Repeat = repeat;
            SetTabName(aitTabsNames.aitTBCalTrainings);
        }

        public aitTrainingModel(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            m_TrainingType = (aitTrainingTypes)info.GetValue("TrainingType", typeof(aitTrainingTypes));
            m_TimeOfDay = (aitTimeOfDay)info.GetValue("TimeOfDay", typeof(aitTimeOfDay));
            m_Series = (uint)info.GetValue("Series", typeof(uint));
            m_Repeat = (uint)info.GetValue("Repeat", typeof(uint));
            m_KM = (uint)info.GetValue("KM", typeof(uint));
            m_Time = (uint)info.GetValue("Time", typeof(uint));
            m_Steps = (uint)info.GetValue("Steps", typeof(uint));
        }

        #endregion
        #region Implementation interface methods

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("TrainingType", m_TrainingType, typeof(aitTrainingTypes));
            info.AddValue("TimeOfDay", m_TimeOfDay, typeof(aitTimeOfDay));
            info.AddValue("Series", m_Series, typeof(uint));
            info.AddValue("Repeat", m_Repeat, typeof(uint));
            info.AddValue("KM", m_KM, typeof(uint));
            info.AddValue("Time", m_Time, typeof(uint));
            info.AddValue("Steps", m_Steps, typeof(uint));
        }

        #endregion
    }
}
