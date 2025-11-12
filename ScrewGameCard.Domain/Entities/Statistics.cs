using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewGameCard.Domain.Entities
{
    public class Statistics
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public virtual Player Player { get; set; }
    }
}
