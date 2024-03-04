using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Uitilities
{
    public static class SD
    {
          public const /*static */ string AdminRole = "Admin";
          public const /*static*/ string EditorRole = "Editor";
        public const /*static*/ string CustomerRole = "Customer";



        //order header
        public const string Pending = "Pending";
        public const string Approve = "Approved";
        public const string Proccessing = "Proccessing";
        public const string Cancelled = "Cancelled";
        public const string Shipped = "Shipped";
        public const string Refund = "Refund";
        public const string Rejected = "Rejected";

        public const string SessionKey = "ShoppingCartSession";

    }
}
