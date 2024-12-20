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
    
    public partial class UserAccounts
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserAccounts()
        {
            this.Artists = new HashSet<Artists>();
            this.Producers = new HashSet<Producers>();
            this.Admins = new HashSet<Admins>();
        }
    
        public int account_id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public int role_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Artists> Artists { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Producers> Producers { get; set; }
        public virtual Roles Roles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Admins> Admins { get; set; }
    }
}
