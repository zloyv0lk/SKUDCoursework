//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SKUD.DataBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class guest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public guest()
        {
            this.guest_event = new HashSet<guest_event>();
        }
    
        public int guest_id { get; set; }
        public string surname { get; set; }
        public string name { get; set; }
        public string patronomyc { get; set; }
        public string passport { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public string telephone { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<guest_event> guest_event { get; set; }
        public virtual guest_type guest_type { get; set; }
    }
}
