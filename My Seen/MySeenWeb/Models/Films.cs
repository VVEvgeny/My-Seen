//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MySeenWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Films
    {
        public int Id { get; set; }
        public string UsersId { get; set; }
        public string Name { get; set; }
        public System.DateTime DateSee { get; set; }
        public int Rate { get; set; }
        public System.DateTime DateChange { get; set; }
    
        public virtual AspNetUsers UserId { get; set; }
    }
}
