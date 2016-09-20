using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PilotKit.Domain.Model.Common
{
    public class PilotKitMenu
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconName { get; set; }
        public long ParentId { get; set; }
        public string RelativeUrl { get; set; }
        public string UrlParams { get; set; }
        public string Role { get; set; }
        public string Category { get; set; }
        public IList<PilotKitMenu> SubPilotKitMenu { get; set; }
    }
}
