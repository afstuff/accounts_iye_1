using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustodianLife.Data
{
    public class MyErrorListing
    {
        List<String> _emsgs;
        public List<String> ErrorMsgs
        {
            get
            {
                return this._emsgs;
            }
            set
            {
                this._emsgs = value;
            }
        }
    }
}
