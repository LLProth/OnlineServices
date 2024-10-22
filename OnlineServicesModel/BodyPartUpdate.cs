using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineServicesModel
{
    public class BodyPartUpdate
    {
        public Boolean UpdateBodyPartsBool { get; set; }
        public Boolean UpdateBodyPartsLocationBool { get; set; }
        public Boolean success { get; set; }
        public string errMsg { get; set; }
    }
}
