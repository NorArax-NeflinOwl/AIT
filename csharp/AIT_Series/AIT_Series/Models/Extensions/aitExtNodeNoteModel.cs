using System;
using System.Collections.Generic;
using System.Linq;
using AIT.Constant;
using System.Runtime.Serialization;
using System.Collections;

namespace AIT.Models
{
    public class aitExtNodeNoteModel : aitExtSimpleNoteModel, ICloneable, ISerializable, IEnumerable<aitExtNodeNoteModel>
    {
        private class aitNodeNoteModelEnum : IEnumerator
        {
            public IList<aitExtNodeNoteModel> m_Children;

            int position = -1;

            public aitNodeNoteModelEnum(IList<aitExtNodeNoteModel> list)
            {
                m_Children = list;
            }

            public bool MoveNext()
            {
                position++;
                return (position < m_Children.Count);
            }

            public void Reset()
            {
                position = -1;
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public aitExtNodeNoteModel Current
            {
                get
                {
                    try
                    {
                        return m_Children[position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }

        #region Properties

        private aitExtNodeNoteModel m_Parent = null;
        public aitExtNodeNoteModel Parent
        {
            get
            {
                return m_Parent;
            }
            set
            {
                m_Parent = value;
            }
        }

        private IList<aitExtNodeNoteModel> m_Children = null;
        public IList<aitExtNodeNoteModel> Children
        {
            get
            {
                if (m_Children == null)
                    return new List<aitExtNodeNoteModel>();
                return m_Children.OrderBy(q => q.Title).ToList();
            }
        }

        public bool IsLeef
        {
            get
            {
                return m_Children == null || m_Children.Count == 0;
            }
        }

        public bool HasChildren
        {
            get
            {
                return m_Children.Count > 0;
            }
        }

        #endregion
        #region Constructors

        public aitExtNodeNoteModel() 
        {
            m_Children = new List<aitExtNodeNoteModel>();
            SetTabName(aitTabsNames.aitTBCalNote);
        }
        public aitExtNodeNoteModel(
            string title, 
            string note, 
            DateTime? noteDate, 
            aitDateTimeType dateType = aitDateTimeType.DATE)
            : base(title, note, noteDate, dateType, aitNotesTypesModel.TREE_VIEW_MODEL)
        {
            m_Children = new List<aitExtNodeNoteModel>();
            SetTabName(aitTabsNames.aitTBCalNote);
        }

        public aitExtNodeNoteModel(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            m_Parent = (aitExtNodeNoteModel)info.GetValue("Parent", typeof(aitExtNodeNoteModel));
            m_Children = (IList<aitExtNodeNoteModel>)info.GetValue("Children", typeof(IList<aitExtNodeNoteModel>));
        }

        #endregion
        #region Public methods

        public aitExtNodeNoteModel Search(aitExtNodeNoteModel node, string id)
        {
            if (node != null)
            {
                if (node.ID.Equals(id))
                    return node;

                if (!node.IsLeef)
                    foreach (var children in node.Children)
                        return Search(children, id);
            }
            throw new ArgumentNullException("Node is not exists");
        }

        #region API

        public void InsertNote(aitExtNodeNoteModel note)
        {
            Children.Add(note);
        }
        public void EditNode(aitExtNodeNoteModel newNode)
        {
            var id = ID;
            var node = this;
            node = newNode;
            SetID(id);
        }
        public void RemoveNode(aitExtNodeNoteModel node)
        {
            node.Parent.Children.Remove(node);
        }
        public void RemoveAllFromList(aitExtNodeNoteModel node)
        {
            if (node == null) return;

            if (!node.IsLeef)
            {
                foreach (var children in node.Children)
                {
                    RemoveAllFromList(children);
                }
                node.Children.Clear();
            }
            else
                node = null;
        }
        
        #endregion
        #endregion
        #region Implementation interface methods

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Parent", m_Parent, typeof(aitExtNodeNoteModel));
            info.AddValue("Children", m_Children, typeof(IList<aitExtNodeNoteModel>));
        }

        public IEnumerator<aitExtNodeNoteModel> GetEnumerator()
        {
            return (IEnumerator<aitExtNodeNoteModel>)GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new aitNodeNoteModelEnum(m_Children);
        }

        #endregion
    }
}
