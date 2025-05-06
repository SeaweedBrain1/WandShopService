using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WandUser.Domain.Exceptions.Register
{
    public class UsernameAlreadyTakenException : Exception
    {
        public UsernameAlreadyTakenException() : base("This username is already taken.")
        {
        }

        public UsernameAlreadyTakenException(string message) : base(message)
        {
        }

        public UsernameAlreadyTakenException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
