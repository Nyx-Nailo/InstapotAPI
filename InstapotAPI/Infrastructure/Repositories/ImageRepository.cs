using Microsoft.EntityFrameworkCore;

namespace InstapotAPI.Infrastructure.Repositories;

public class ImageRepository
{
    DbContext _dbContext;
    public ImageRepository(DbContext context)
    {
        _dbContext = context;
    }

    public bool CreateNewImage(int userID, string path, string title, string description, DateTime createdDate)
    {
        return true;
    }

}
