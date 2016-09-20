using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PilotKit.Infrastructure.Interfaces.ExcelImport
{
    public abstract class ExcelModelBase
    {
        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }
    }
}
