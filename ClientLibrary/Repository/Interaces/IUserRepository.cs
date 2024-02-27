using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary.Repository.Interaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();
        Task<User> FindById(int id);
    }
}
