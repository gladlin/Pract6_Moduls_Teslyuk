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
    
    public partial class Albums
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Albums()
        {
            this.ProductionRequests = new HashSet<ProductionRequests>();
            this.RevenueReports = new HashSet<RevenueReports>();
            this.Shipments = new HashSet<Shipments>();
            this.ContractsWithManufacturers = new HashSet<ContractsWithManufacturers>();
        }
    
        public int album_id { get; set; }
        public int artist_id { get; set; }
        public string title { get; set; }
        public string genre { get; set; }
        public Nullable<System.DateTime> release_date { get; set; }
        public string cover { get; set; }
    
        public virtual Artists Artists { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductionRequests> ProductionRequests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RevenueReports> RevenueReports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Shipments> Shipments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContractsWithManufacturers> ContractsWithManufacturers { get; set; }
    }
}
