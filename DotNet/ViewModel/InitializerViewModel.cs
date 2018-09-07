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
        public int plageEstimation { get; set; }
        public double strike { get; set; }
        public InitializerViewModel()
        {
            debutTest = DateTime.Now;
            plageEstimation = 15;
            strike = 8;
        }
        #endregion
        public DateTime DebutTest
        {
            get { return debutTest; }
            set { debutTest = value; }
        }

        public int PlageEstimation
        {
            get { return plageEstimation; }
            set { plageEstimation = value; }
        }

        public double Strike
        {
            get { return strike; }
            set { strike = value; }
        }
    }
}
