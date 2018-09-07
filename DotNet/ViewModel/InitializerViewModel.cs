using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.ViewModel
{
    class InitializerViewModel
    {
        #region public properties
        public DateTime debutTest { get; set; }
        public InitializerViewModel()
        {
                debutTest = DateTime.Now;
        }
        #endregion
        public DateTime DebutTest
        {
            get { return debutTest; }
            set { debutTest = value; }
        }
    }
}
