//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Final_Property_Rental.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Appointment
    {
        public int AppointmentId { get; set; }
        public int TenantId { get; set; }
        public int ManagerId { get; set; }
        public System.DateTime AppointmentDate { get; set; }
        public string Status { get; set; }
    
        public virtual PotentialTenant PotentialTenant { get; set; }
        public virtual PropertyManager PropertyManager { get; set; }
    }
}
