//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Практическая_3.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Admins
    {
        public int admin_id { get; set; }
        public int account_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    
        public virtual UserAccounts UserAccounts { get; set; }
    }
}
