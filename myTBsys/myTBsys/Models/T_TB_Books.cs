//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace myTBsys.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class T_TB_Books
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public T_TB_Books()
        {
            this.T_TB_Choose = new HashSet<T_TB_Choose>();
            this.T_TB_Fenfa = new HashSet<T_TB_Fenfa>();
            this.T_TB_TeaYuding = new HashSet<T_TB_TeaYuding>();
            this.T_TB_StuYuding = new HashSet<T_TB_StuYuding>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> Edition { get; set; }
        public string Author { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Publisher { get; set; }
        public Nullable<int> Store { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_TB_Choose> T_TB_Choose { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_TB_Fenfa> T_TB_Fenfa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_TB_TeaYuding> T_TB_TeaYuding { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_TB_StuYuding> T_TB_StuYuding { get; set; }
    }
}