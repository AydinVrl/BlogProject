using Entity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Abstract
{
    public class BaseClass
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public Status Status { get; set; } = Status.Active;
    }
}
