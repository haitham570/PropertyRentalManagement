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
    
    public partial class Apartment
    {
        public int ApartmentId { get; set; }
        public int BuildingId { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public string Status { get; set; }
    
        public virtual Building Building { get; set; }
    }
}