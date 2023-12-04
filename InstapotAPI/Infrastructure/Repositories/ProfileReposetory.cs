using InstapotAPI.Entity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace InstapotAPI.Infrastructure.Repositories
{
    public class ProfileReposetory
    {
        private InstapotContext _context;
        
        public ProfileReposetory(InstapotContext context)
        {
            _context = context;
        }

        public async Task<Profile> Create(Profile newProfile)
        {
            _context.Add(newProfile);
            await _context.SaveChangesAsync();

            return newProfile;
        }

        public async Task<Profile> Delete(int id)
        {
            var removedProfile = await _context.Profiles.FindAsync(id);
            
            if (removedProfile != null)
            {
                _context.Profiles.Remove(removedProfile);
                _context.SaveChangesAsync();
            }

            return removedProfile;
        }

        public async Task<string> PathToProfileImage(int id)
        {
            var profileImagePath = await _context.Profiles.FindAsync(id);
            
            if (profileImagePath == null)
            {
                return null;
            }

            return profileImagePath.ProfilePicture;
        }

        public async Task<Profile> UpdatePathToProfileImage(Profile newPathToProfileImage)
        {
            var updatePathToProfileImage = await _context.Profiles.FindAsync(newPathToProfileImage.Id);

            if (updatePathToProfileImage != null)
            {
                updatePathToProfileImage.ProfilePicture = newPathToProfileImage.ProfilePicture;
                await _context.SaveChangesAsync();
            }

            return updatePathToProfileImage;
        }

        public async Task<Profile> UpdateUsername(Profile newUsername)
        {
            var updateUsername = await _context.Profiles.FindAsync(newUsername.Id);

            if (updateUsername != null)
            {
                updateUsername.Username = newUsername.Username;
                await _context.SaveChangesAsync();
            }

            return updateUsername;
        }

        public async Task<Profile> UpdatePassword(Profile newPassword)
        {
            var updatePassword = await _context.Profiles.FindAsync(newPassword.Id);

            if (updatePassword != null)
            {
                updatePassword.Password = newPassword.Password;
                await _context.SaveChangesAsync();
            }

            return updatePassword;
        }

    }
}
