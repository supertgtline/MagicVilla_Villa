using System.Linq.Expressions;
using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.Repository.IRepository
{

    public interface IVillaRepository
    {
        /// <summary>
        /// Get All Villa
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<List<Villa>> GetAllAsync(Expression<Func<Villa,bool>> filter = null);
        Task<Villa> GetAsync(Expression<Func<Villa,bool>> filter = null,bool tracked = true);
        Task CreateAsync(Villa entity);
        Task UpadateAsync(Villa entity);
        Task RemoveAsync(Villa entity);
        Task SaveAsync();
    }
}