using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jupiter.Repository.Models;

namespace Jupiter.Repository.Interfaces
{
    public interface IRepository<T> where T : DatabaseObject
    {
        /// <summary>
        /// Adds an entry to the database. Doesn't check if this entry already exists and doesn't care about the ID property.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        Task AddEntry(T entry);
        /// <summary>
        /// Removes all entries in the database with identical properties (except ID)
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        Task RemoveEntry(T entry);
        /// <summary>
        /// Removes an entry from the database based on its ID.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Task RemoveEntry(int ID);
        /// <summary>
        /// Edits an entry of a specific ID.
        /// </summary>
        /// <param name="ID">ID of an entry</param>
        /// <param name="newEntry">New parameters for the entry</param>
        /// <returns></returns>
        Task EditEntry(T entry);
        Task<T> GetEntry(int ID);
        Task<IEnumerable<T>> GetAllEntries();
    }
}
