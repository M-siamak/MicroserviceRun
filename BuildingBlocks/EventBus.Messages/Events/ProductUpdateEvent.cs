using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class ProductUpdateEvent : IntegrationBaseEvent
    {
        public string ProductName { get; set; }
        public string NewProductName { get; set; }
    }
}
