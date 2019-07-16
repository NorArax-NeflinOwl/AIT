using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIT.Models;

namespace AIT.Interface
{
    interface aitMTreeInterface
    {
        aitExtNodeNoteModel Root { get; }
        bool IsEmpty { get; }

        aitExtNodeNoteModel Search(string key);
        void Insert(aitExtNodeNoteModel node);
        void Edit(aitExtNodeNoteModel oldNode, aitExtNodeNoteModel newNode);
        void Remove(aitExtNodeNoteModel node);
        void Clear();
    }
}
