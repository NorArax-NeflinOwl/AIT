using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIT_Lib.FilesManagers;

namespace AIT_Lib.Base
{
    public static class aitInitializer
    {
        public static void Run()
        {
            aitFileManager.Init();
        }
    }
}
