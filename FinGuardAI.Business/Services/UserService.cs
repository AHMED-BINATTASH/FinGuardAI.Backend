using FinGuardAI.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Infrastructure.Persistence.Repositories;

namespace FinGuardAI.Business.Services
{
    public class UserService
    {
        private UserRepository _UserRepository;

        public UserService(UserRepository UserRepository)
        {
            _UserRepository = UserRepository;

        }
        async public Task<IEnumerable<User>?> GetAll()
        {
            IEnumerable<User> Users = await _UserRepository.GetAllAsync();

            return (Users);
        }
        async public Task<User?> GetByID(int id)
        {
            var User = await _UserRepository.GetByIdAsync(id);

            return User;

        }
        async public Task<bool> AddNew(User Entity)
        {
            return await _UserRepository.Add(Entity);
        }

        async public Task<bool> Delete(int id)
        {
            return await _UserRepository.Delete(id);
        }


        public async Task<bool> Update(User Entity)
        {
            return await _UserRepository.Update(Entity);
        }


        public async Task<bool> IsUsernameExist(string username)
        {
            return await _UserRepository.IsUsernameExistAsync(username);
        }

        public async Task<bool> IsPersonExist(int PersonID)
        {
            return await _UserRepository.IsPersonExistAsync(PersonID);
        }
    }
}
