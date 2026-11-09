using FluentValidation;
using OneMusic.EntityLayer.Entities;

namespace OneMsuic.BusinessLayer.Validators
{
    public class PlaylistValidator : AbstractValidator<Playlist>
    {
        public PlaylistValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Çalma listesi adı boş bırakılamaz").MaximumLength(100).WithMessage("Çalma listesi adı en fazla 100 karakter olabilir");

            RuleFor(x => x.Description).MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir");
        }
    }
}
