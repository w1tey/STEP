using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkProject.DbModels
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Government { get; set; }
        public string ImageURL { get; set; }
        public int Population { get; set;}
        public float Area { get; set; }
        public float GDP { get; set; }

    }

}