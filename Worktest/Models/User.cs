using System;
using System.Collections.Generic;

namespace Worktest.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string FIO { get; set; }
        public DateTime? Birth { get; set; }
        public string Phone { get; set; }
    }
}
