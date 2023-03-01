using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repositories.Repositories.Like.Extensions;
using Repositories.Repositories.Like.Models;
using Spotify.Shared.DAL.Like;
using DALModels = Spotify.Shared.DAL.Like.Models;

namespace Repositories.Repositories.Like;

public class LikeRepository : ILikeRepository
{
    private Contexts.SpotifyContext Context { get; }

    public LikeRepository(IConfiguration configuration)
    {
        var optionsBuilder =
            new DbContextOptionsBuilder<Contexts.SpotifyContext>().UseNpgsql(
                configuration.GetConnectionString("DBContext"));
        Context = new Contexts.SpotifyContext(optionsBuilder.Options);
    }

    ~LikeRepository()
    {
        Context.Dispose();
    }

    public async Task<DALModels.Like> SetAsync(string associatedId, string associatedType, string associatedUser)
    {
        var likeInDb = await Context.Likes.Where(like => like.AssociatedId == associatedId).FirstOrDefaultAsync();
        if (likeInDb != null)
        {
            return likeInDb.ToDalLike();
        }

        var res = await Context.Likes.AddAsync(new Contexts.Like
        {
            AssociatedId = associatedId,
            AssociatedType = associatedType,
            AssociatedUser = associatedUser
        });
        await Context.SaveChangesAsync();

        return res.Entity.ToDalLike();
    }

    public async Task DeleteAsync(string id, string associatedUserId)
    {
        var res = await Context.Likes.Where(like => like.Id == new Guid(id) && like.AssociatedUser == associatedUserId)
            .FirstOrDefaultAsync();
        if (res == null)
        {
            return;
        }

        Context.Likes.Remove(res);
        await Context.SaveChangesAsync();
    }

    public async Task<DALModels.Like?> GetByAssociatedUserIdAsync(string id, string associatedUserId)
    {
        var res = await Context.Likes.Where(
                like => like.AssociatedId == id && like.AssociatedUser == associatedUserId)
            .FirstOrDefaultAsync();
        if (res == null)
        {
            return null;
        }
        return res.ToDalLike();
    }

    public async Task<DALModels.FindLikesByUserIdResult> FindLikesByUserId(
        string userId,
        DALModels.FindLikesByUserIdOptions options
    )
    {
        var associatedTypes = options.AssociatedTypes?.ToStringArray();
        var total = await Context.Likes
            .Where(like => like.AssociatedUser == userId)
            .Where(like => associatedTypes == null || associatedTypes.Contains(like.AssociatedType))
            .CountAsync();
        var res = await Context.Likes
            .Where(like => like.AssociatedUser == userId)
            .Where(like => associatedTypes == null || associatedTypes.Contains(like.AssociatedType))
            .Skip(options.Pagination.Offset)
            .Take(options.Pagination.Limit)
            .ToArrayAsync();
        var likes = res.Select(like => like.ToDalLike());
        return new DALModels.FindLikesByUserIdResult(
            likes,
            options.Pagination.Limit,
            options.Pagination.Offset,
            total
        );
    }

    public async Task<int> GetLikedTracksCountByUserId(string userId)
    {
        var res = await Context.Likes
            .Where(like => like.AssociatedUser == userId && like.AssociatedType == AssociatedTypes.Track)
            .CountAsync();
        return res;
    }
}