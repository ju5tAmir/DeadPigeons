using System.Security.Claims;
using DataAccess.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Service.Authorization;
using Service.Preference.Dto;
using Service.Repositories;
using Service.Security;

namespace Service.Preference
{
    public class PreferenceService : IPreferenceService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<DataAccess.Entities.Preference> _preferenceRepository;
        

        public PreferenceService(
            IRepository<User> userRepository,
            IRepository<DataAccess.Entities.Preference> preferenceRepository
            )
        {
            _userRepository = userRepository;
            _preferenceRepository = preferenceRepository;
        }

        public PreferenceResponse GetById(Guid userId, ClaimsPrincipal principal)
        {
          
            AuthorizationService.AuthorizeAndThrow(principal, userId);

            var preference = _preferenceRepository
                .Query()
                .Where(p => p.UserId == userId.ToString())
                .Select(p => new PreferenceResponse(
                    p.IfBalanceIsNegative,
                    p.IfPlayerWon,
                    p.NotificationType
                ))
                .FirstOrDefault();
            
            if (preference == null)
            {
                throw new KeyNotFoundException();
            }

            return preference;
        }

        public Task Update(Guid preferenceId, Guid userId, ClaimsPrincipal principal, PreferenceUpdateRequest data)
        {
            throw new NotImplementedException();
        }
    }
}
