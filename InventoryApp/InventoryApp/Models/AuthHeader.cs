using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Web.Services.Protocols;

namespace InventoryApp.Models
{   
    public class AuthHeader : SoapHeader
    {
        public string Username;
        public string Password;
    }
}
