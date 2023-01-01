using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repositories.Contexts;
using Spotify.Shared.DAL.Like;
using DALModels = Spotify.Shared.DAL.Like.models;

namespace Repositories.Repositories;

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
            return new DALModels.Like(
                likeInDb.Id.ToString(),
                likeInDb.AssociatedId,
                likeInDb.AssociatedUser,
                likeInDb.AssociatedType
            );
        }

        var res = await Context.Likes.AddAsync(new Like
        {
            AssociatedId = associatedId,
            AssociatedType = associatedType,
            AssociatedUser = associatedUser
        });
        await Context.SaveChangesAsync();
        return new DALModels.Like(
            res.Entity.Id.ToString(),
            res.Entity.AssociatedId,
            res.Entity.AssociatedUser,
            res.Entity.AssociatedType
        );
    }

    public async Task DeleteAsync(string id)
    {
        var res = await Context.Likes.FindAsync(new Guid(id));
        if (res == null)
        {
            return;
        }

        Context.Likes.Remove(res);
        await Context.SaveChangesAsync();
    }

    public async Task<DALModels.Like?> GetByAssociatedIdAsync(string id)
    {
        var res = await Context.Likes.Where(like => like.AssociatedId == id)
            .FirstOrDefaultAsync();
        if (res == null)
        {
            return null;
        }

        return new DALModels.Like(
            res.Id.ToString(),
            res.AssociatedId,
            res.AssociatedUser,
            res.AssociatedType
        );
    }

    public async Task<DALModels.UserLikes> GetByAssociatedUserId(string id,
        DALModels.UserLikesRequest? userLikesRequest = null)
    {
        var limit = userLikesRequest?.Limit ?? 10;
        var offset = userLikesRequest?.Offset ?? 0;
        var baseQuery = Context.Likes
            .Where(like => like.Id == new Guid(id));
        var likesQueryTask = baseQuery
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
        var totalTask = baseQuery
            .CountAsync();
        await Task.WhenAll(likesQueryTask, totalTask);
        var likes = await likesQueryTask;
        var total = await totalTask;
        var mappedLikes = likes.Select(like =>
            new DALModels.Like(like.Id.ToString(), like.AssociatedId, like.AssociatedUser, like.AssociatedType)
        ).ToArray();
        return new DALModels.UserLikes(mappedLikes, limit, offset, total);
    }
}