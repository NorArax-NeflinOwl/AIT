using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIT.Models;
using AIT.Interface;

namespace AIT.Structures
{
    public class aitExtMTree : aitMTreeInterface
    {
        private aitExtNodeNoteModel m_Root;
        public aitExtNodeNoteModel Root 
        {
            get
            {
                return m_Root;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return m_Root == null;
            }
        }

        private aitExtNodeNoteModel Search(aitExtNodeNoteModel node, string key)
        {
            if (node == null)
                return null;

            if (node.Title.Equals(key))
                return node;

            foreach (aitExtNodeNoteModel child in node.Children)
            {
                var found = Search(child, key);
                if (found != null)
                    return found;
            }

            return null;
        }
        public aitExtNodeNoteModel Search(string key)
        {
            return Search(m_Root, key);
        }

        public void Insert(aitExtNodeNoteModel node)
        {
            throw new NotImplementedException();
        }

        public void Edit(aitExtNodeNoteModel oldNode, aitExtNodeNoteModel newNode)
        {
            oldNode.EditNode(newNode);
            oldNode.Update();
        }

        public void Remove(aitExtNodeNoteModel node)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            m_Root = null;
        }
    }
}
