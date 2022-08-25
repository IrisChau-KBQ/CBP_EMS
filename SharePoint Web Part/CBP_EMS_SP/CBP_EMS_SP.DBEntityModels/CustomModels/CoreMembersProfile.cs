namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Serializable]
    public class CoreMembersProfile
    {
        public CoreMembersProfile() { }
        public string Name { get; set; }
        public string Positions { get; set; }
        public string Academic { get; set; }
        public string Experiences { get; set; }
        public string Achievements { get; set; }


    }
}
