using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlValidatorExe
{
    internal enum ReturnCode
    {
        NotSet = -1,
        Ok = 0,
        ErrorReadingParameters = 2,
        ErrorReadingXsdFile = 3,
        InvalidParameters = 4, 
        ValidationErrors = 5
    }
}
