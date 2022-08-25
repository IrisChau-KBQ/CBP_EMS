using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBP_EMS_SP.Data.Models
{
    public  class CompanyProfiles
    {

        public Guid Company_Profile_ID { get; set; }

        [StringLength(255)]
        public string Name_Eng { get; set; }

        [StringLength(255)]
        public string Name_Chi { get; set; }

        [StringLength(50)]
        public string Applicaition_Type { get; set; }

    }
}
