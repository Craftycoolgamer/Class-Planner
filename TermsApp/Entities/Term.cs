using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TermsApp.Entities
{
    [Table("Terms")]
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        
        [Column("Id")]
        public int Id { get; set; }

        [Column("Name")]
        public string? Name { get; set; }

        [Column("Start Date")]
        public DateTime StartDate { get; set; }

        [Column("End Date")]
        public DateTime EndDate { get; set; }
    }
}
