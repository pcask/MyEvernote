using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Common
{
    public static class App
    {
        // Field'ın default değeri atanıyor.
        public static ICommon common = new DefaultCommon();
    }
}
