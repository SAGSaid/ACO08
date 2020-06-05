using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACO08_Library.Data.Validation
{
    interface IValidateData<in T>
    {
        bool Validate(T data);
    }
}
