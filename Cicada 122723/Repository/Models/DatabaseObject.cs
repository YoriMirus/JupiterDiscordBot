using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jupiter.Repository.Models
{
    /// <summary>
    /// Repressents an object that can be stored inside the database.
    /// </summary>
    public abstract class DatabaseObject
    {
        /// <summary>
        /// ID of this object inside the database.
        /// </summary>
        public int ID { get; set; }
    }
}
