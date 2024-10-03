using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace InventoryApp.BLL
{
    public class SecurityWebService
    {              
       private static inventoryWebService.AuthHeader authHead;    
        
        // AuthHeaders  
        private static string user = "myuser";
        private static string password = "mypassword";

        static SecurityWebService()
        {
            authHead = new inventoryWebService.AuthHeader();  
            authHead.Username = user; 
            authHead.Password = password; 
        }

        public static inventoryWebService.AuthHeader authData
        {
            get { return authHead; }
        }
    }
}
