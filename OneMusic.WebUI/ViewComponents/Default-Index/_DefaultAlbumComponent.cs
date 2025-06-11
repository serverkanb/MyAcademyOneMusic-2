using Microsoft.AspNetCore.Mvc;
using OneMsuic.BusinessLayer.Abstract;
using OneMusic.EntityLayer.Entities;

namespace OneMusic.WebUI.ViewComponents.Default_Index
{
    //.net 8 le gelen constructerı tanımlama şekli
    public class _DefaultAlbumComponent: ViewComponent

    {

        private readonly IAlbumService _albumService;

        public _DefaultAlbumComponent(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        public IViewComponentResult Invoke()
        {

            var values = _albumService.TGetAlbumswithArtist();

            if (values == null || !values.Any())
            {
                values = new List<Album>(); //Boş Liste döndür
            }
            return View(values);
        }
    }
}
