using System.Text.RegularExpressions;
using UserApi.Exceptions;
using UserApi.Models;

namespace UserApi.Services
{
    public static class ValidationService
    {
        const string emailPattern = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        const string fioPattern = @"^[а-яА-ЯеЁ]+$";

        private static bool IsEmptyString(this string str)
        {
            if (str == null || str.Length == 0)
                return true;
            return false;
        }

        public static void ValidateUser(User user)
        {
            if (user.Name.IsEmptyString())
                throw new UserValidationException("Не заполнено поле Имя");
            if (user.SecondName.IsEmptyString())
                throw new UserValidationException("Не заполнено поле Фамилия");
            if (user.Email.IsEmptyString())
                throw new UserValidationException("Не заполнено поле Почта");

            if (!Regex.Match(user.Name, fioPattern).Success)
                throw new UserValidationException("Неправильно указано поле Имя");

            if (!Regex.Match(user.SecondName, fioPattern).Success)
                throw new UserValidationException("Неправильно указано поле Фамилия");

            if (!Regex.Match(user.Email, emailPattern).Success)
                throw new UserValidationException("Неправильно указано поле Почта");
        }
    }
}
