using Microsoft.AspNetCore.Identity;
using OneMsuic.BusinessLayer.Abstract;
using OneMusic.EntityLayer.Entities;

namespace OneMusic.WebUI.Services
{
    // Giriş yapan kullanıcının favori şarkı id'lerini istek başına bir kez yükler.
    // View'larda kalp ikonunun dolu/boş görünmesi için kullanılır.
    public class FavoriteState
    {
        private readonly IFavoriteSongService _favoriteSongService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private HashSet<int>? _favoriteIds;

        public FavoriteState(IFavoriteSongService favoriteSongService, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _favoriteSongService = favoriteSongService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsSignedIn => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;

        public bool IsFavorite(int songId)
        {
            if (_favoriteIds == null)
            {
                _favoriteIds = new HashSet<int>();
                var user = _httpContextAccessor.HttpContext?.User;
                if (user != null && int.TryParse(_userManager.GetUserId(user), out var userId))
                {
                    _favoriteIds = _favoriteSongService.TGetFavoriteSongIds(userId).ToHashSet();
                }
            }
            return _favoriteIds.Contains(songId);
        }
    }
}
