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
    
    public partial class FinancialReports
    {
        public int report_id { get; set; }
        public int studio_id { get; set; }
        public Nullable<decimal> total_expenses { get; set; }
        public Nullable<decimal> total_revenue { get; set; }
        public Nullable<System.DateTime> report_date { get; set; }
    
        public virtual Studios Studios { get; set; }
    }
}
