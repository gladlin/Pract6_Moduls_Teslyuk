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
    
    public partial class Artists
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Artists()
        {
            this.AlbumRequests = new HashSet<AlbumRequests>();
            this.Albums = new HashSet<Albums>();
            this.RoomSchedules = new HashSet<RoomSchedules>();
        }
    
        public int artist_id { get; set; }
        public int account_id { get; set; }
        public int producer_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone_number { get; set; }
        public string passport_number { get; set; }
        public string middle_name { get; set; }
        public string photo_path { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlbumRequests> AlbumRequests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Albums> Albums { get; set; }
        public virtual UserAccounts UserAccounts { get; set; }
        public virtual Producers Producers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoomSchedules> RoomSchedules { get; set; }
    }
}
