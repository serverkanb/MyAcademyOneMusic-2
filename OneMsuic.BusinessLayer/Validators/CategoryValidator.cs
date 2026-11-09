using FluentValidation;
using OneMusic.EntityLayer.Entities;

namespace OneMsuic.BusinessLayer.Validators
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Kategori adı boş bırakılamaz").MaximumLength(50).WithMessage("En Fazla 50 karakter yazabilirsiniz.");
        }
    }
}
