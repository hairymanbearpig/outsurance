using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileParser
{
    public class Person
    {
        public string FirstName;
        public string LastName;
        public string Address;
        public string PhoneNo;

        public string StreetName
        {
            get {
                int space = this.Address.IndexOf(" ")+1;
                return this.Address.Substring(space);
            }
        }
        public string StreetNo
        {
            get
            {
                int space = this.Address.IndexOf(" ")-1;
                return this.Address.Substring(0,space);
            }
        }
    }
}
