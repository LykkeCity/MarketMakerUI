using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LykkeMarketMakers.Core.DomainModels;
using LykkeMarketMakers.Core.Interfaces.DomainModels;
using LykkeMarketMakers.Core.Interfaces.Repositories;

namespace LykkeMarketMakers.Web.Services
{
    public class UserCacheService : IUserCacheService
    {
        private readonly IBackOfficeUsersRepository _usersRepository;
        private readonly IBackofficeUserRolesRepository _rolesRepository;
        private static DateTime _lastCacheUpdated = new DateTime(2010, 1, 1);
        private static Dictionary<string, IBackOfficeUser> _usersCache = new Dictionary<string, IBackOfficeUser>();
        private static IBackofficeUserRole[] _rolesCache = new IBackofficeUserRole[0];

        public UserCacheService(
            IBackOfficeUsersRepository usersRepository, 
            IBackofficeUserRolesRepository rolesRepository)
        {
            _usersRepository = usersRepository;
            _rolesRepository = rolesRepository;
        }

        public async Task UpdateUsersAndRoles()
        {
            if ((DateTime.UtcNow - _lastCacheUpdated).TotalSeconds > 30)
            {
                _usersCache = (await _usersRepository.GetAllAsync()).ToDictionary(itm => itm.Id);
                _rolesCache = (await _rolesRepository.GetAllRolesAsync()).Select(BackofficeUserRole.Create).Cast<IBackofficeUserRole>().ToArray();
                _lastCacheUpdated = DateTime.UtcNow;
            }
        }

        public UserRolesPair GetUsersRolePair(string userId)
        {
            if (!_usersCache.ContainsKey(userId))
            {
                return new UserRolesPair
                {
                    User = null,
                    Roles = null
                };
            }

            return new UserRolesPair
            {
                User = _usersCache[userId],
                Roles = _rolesCache
            };
        }
    }
}
