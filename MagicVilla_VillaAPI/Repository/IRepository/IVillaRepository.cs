using System.Linq.Expressions;
using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.Repository.IRepository
{

    public interface IVillaRepository: IRepository<Villa>
    {
        /// <summary>
        /// Get All Villa
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<Villa> UpdateAsync(Villa entity);
    }
}