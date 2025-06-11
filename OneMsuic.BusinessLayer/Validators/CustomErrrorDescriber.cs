using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMsuic.BusinessLayer.Validators
{
	public class CustomErrrorDescriber : IdentityErrorDescriber // bu sınıfta kayıt olurkenki hatalar gereklilkler
	{

		public override IdentityError PasswordTooShort(int length)
		{
			return new IdentityError()
			{
				Description="Şifre en az 6 karakterden oluşmalıdır"
			};
		}

		public override IdentityError PasswordRequiresDigit()
		{
			return new IdentityError()
			{
				Description="Şifre en az bir rakam içermelidir"
			};
		}
		public override IdentityError PasswordRequiresLower()
		{
			return new IdentityError()
			{
				Description = "Şifre en az bir büyük harf(A-Z) içermelidir"
			};
		}
		public override IdentityError PasswordRequiresUpper()
		{
			return new IdentityError()
			{
				Description = "Şifre en az bir büyük harf(A-Z) içermelidir"
			};
		}

		public override IdentityError PasswordRequiresNonAlphanumeric()
		{
			return new IdentityError()
			{
				Description = "Şifre en az bir büyük özel karakter(*,+,.,-,_,....) içermelidir."
			};
		}
	}
}
