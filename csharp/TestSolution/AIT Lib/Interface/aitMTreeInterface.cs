using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIT_Lib.Models;

namespace AIT_Lib.Interface
{
    interface aitMTreeInterface
    {
        aitNodeNoteModel Root { get; }
        bool IsEmpty { get; }

        aitNodeNoteModel Search(string key);
        void Insert(aitNodeNoteModel node);
        void Edit(aitNodeNoteModel oldNode, aitNodeNoteModel newNode);
        void Remove(aitNodeNoteModel node);
        void Clear();
    }
}
