﻿using InstapotAPI.Entity;
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
            newProfile.CreatedDate = DateTime.Now;
            newProfile.IsVerified = false;
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
        
        public async Task<Profile> Profile(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            return profile;
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

        public async Task<Profile> UpdateEmail(Profile newEmail)
        {
            var updateEmail = await _context.Profiles.FindAsync(newEmail.Id);

            if (updateEmail != null)
            {
                updateEmail.Email = newEmail.Email;
                await _context.SaveChangesAsync();
            }

            return updateEmail;
        }

        public async Task<Profile> Verified(Profile profile)
        {
            var confirmedProfile = await _context.Profiles.FindAsync(profile.Id);

            if (confirmedProfile != null)
            {
                confirmedProfile.IsVerified = true;
                await _context.SaveChangesAsync();
            }

            return confirmedProfile;
        }

        public async Task<string?> PathToProfileImage(int id)
        {
            var pathToProfileImage = await _context.Profiles.FindAsync(id);

            if (pathToProfileImage == null || pathToProfileImage.ProfilePicture == null)
            {
                return null;
            }

            return pathToProfileImage.ProfilePicture;
        }

        public async Task<bool?> IsVerified(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);

            if (profile == null)
            {
                return null;
            }

            profile.IsVerified = true;
            await _context.SaveChangesAsync();

            return profile.IsVerified;

        }
    }
}