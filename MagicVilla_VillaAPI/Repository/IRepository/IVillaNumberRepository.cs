using System.Linq.Expressions;
using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.Repository.IRepository
{

    public interface IVillaNumberRepository: IRepository<VillaNumber>
    {
        /// <summary>
        /// Get All Villa
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<VillaNumber> UpdateAsync(VillaNumber entity);
    }
}