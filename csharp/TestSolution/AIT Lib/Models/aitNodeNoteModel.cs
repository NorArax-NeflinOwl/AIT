using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIT_Lib.Constant;
using System.Runtime.Serialization;
using System.Collections;

namespace AIT_Lib.Models
{
    public class aitNodeNoteModel : aitSimpleNoteModel, ICloneable, ISerializable, IEnumerable<aitNodeNoteModel>
    {
        private class aitNodeNoteModelEnum : IEnumerator
        {
            public IList<aitNodeNoteModel> m_Children;

            int position = -1;

            public aitNodeNoteModelEnum(IList<aitNodeNoteModel> list)
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

            public aitNodeNoteModel Current
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

        private aitNodeNoteModel m_Parent = null;
        public aitNodeNoteModel Parent
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

        private IList<aitNodeNoteModel> m_Children = null;
        public IList<aitNodeNoteModel> Children
        {
            get
            {
                if (m_Children == null)
                    return new List<aitNodeNoteModel>();
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

        public aitNodeNoteModel() 
        {
            m_Children = new List<aitNodeNoteModel>();
            SetTabName(aitTabsNames.aitTBCalNote);
        }
        public aitNodeNoteModel(
            string title, 
            string note, 
            DateTime? noteDate, 
            aitDateTimeType dateType = aitDateTimeType.DATE)
            : base(title, note, noteDate, dateType, aitNotesTypesModel.TREE_VIEW_MODEL)
        {
            m_Children = new List<aitNodeNoteModel>();
            SetTabName(aitTabsNames.aitTBCalNote);
        }

        public aitNodeNoteModel(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            m_Parent = (aitNodeNoteModel)info.GetValue("Parent", typeof(aitNodeNoteModel));
            m_Children = (IList<aitNodeNoteModel>)info.GetValue("Children", typeof(IList<aitNodeNoteModel>));
        }

        #endregion
        #region Public methods

        public aitNodeNoteModel Search(aitNodeNoteModel node, string id)
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

        public void InsertNote(aitNodeNoteModel note)
        {
            Children.Add(note);
        }
        public void EditNode(aitNodeNoteModel newNode)
        {
            var id = ID;
            var node = this;
            node = newNode;
            SetID(id);
        }
        public void RemoveNode(aitNodeNoteModel node)
        {
            node.Parent.Children.Remove(node);
        }
        public void RemoveAllFromList(aitNodeNoteModel node)
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
            info.AddValue("Parent", m_Parent, typeof(aitNodeNoteModel));
            info.AddValue("Children", m_Children, typeof(IList<aitNodeNoteModel>));
        }

        public IEnumerator<aitNodeNoteModel> GetEnumerator()
        {
            return (IEnumerator<aitNodeNoteModel>)GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new aitNodeNoteModelEnum(m_Children);
        }

        #endregion
    }
}
