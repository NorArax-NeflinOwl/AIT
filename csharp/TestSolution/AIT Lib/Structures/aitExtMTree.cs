using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIT_Lib.Models;
using AIT_Lib.Interface;

namespace AIT_Lib.Structures
{
    public class aitExtMTree : aitMTreeInterface
    {
        private aitNodeNoteModel m_Root;
        public aitNodeNoteModel Root 
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

        private aitNodeNoteModel Search(aitNodeNoteModel node, string key)
        {
            if (node == null)
                return null;

            if (node.Title.Equals(key))
                return node;

            foreach (aitNodeNoteModel child in node.Children)
            {
                var found = Search(child, key);
                if (found != null)
                    return found;
            }

            return null;
        }
        public aitNodeNoteModel Search(string key)
        {
            return Search(m_Root, key);
        }

        public void Insert(aitNodeNoteModel node)
        {
            throw new NotImplementedException();
        }

        public void Edit(aitNodeNoteModel oldNode, aitNodeNoteModel newNode)
        {
            oldNode.EditNode(newNode);
            oldNode.Update();
        }

        public void Remove(aitNodeNoteModel node)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            m_Root = null;
        }
    }
}
