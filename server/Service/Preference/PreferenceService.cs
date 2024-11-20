using System.Security.Claims;
using DataAccess.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Service.Authorization;
using Service.Preference.Dto;
using Service.Repositories;

namespace Service.Preference
{
    public class PreferenceService(
        IRepository<DataAccess.Entities.Preference> preferenceRepository,
        IValidator<PreferenceUpdateRequest> validator,
        IAuthority authority
        ) : IPreferenceService
    {
        
        public PreferenceResponse GetById(Guid userId, ClaimsPrincipal principal)
        {
          
            authority.AuthorizeAndThrow(principal, userId);

            var preference = preferenceRepository
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

        public async Task<PreferenceResponse> Update(Guid userId, ClaimsPrincipal principal, PreferenceUpdateRequest data)
        { 
            await validator.ValidateAndThrowAsync(data);
            authority.AuthorizeAndThrow(principal, userId);

            var preference = await preferenceRepository
                .Query()
                .Where(p => p.UserId == userId.ToString())
                .FirstOrDefaultAsync();

            if (preference == null)
            {
                throw new NotFoundError(nameof(PreferenceUpdateRequest), new { Id = userId });
            }

            preference.IfBalanceIsNegative = data.ifBalanceIsNegative;
            preference.IfPlayerWon = data.ifPlayerWon;
            preference.NotificationType = data.NotificationType;
            await preferenceRepository.Update(preference);

            return new PreferenceResponse(preference.IfBalanceIsNegative, preference.IfPlayerWon, preference.NotificationType);
        }
    }
}
