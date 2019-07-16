using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIT.FilesManagers;

namespace AIT.Base
{
    public static class aitInitializer
    {
        public static void Run()
        {
            aitFileManager.Init();
        }
    }
}
