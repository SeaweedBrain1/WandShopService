using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WandUser.Domain.Exceptions.Register
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException() : base("User with the same e-mail already exists.") 
        {
        }

        public UserAlreadyExistsException(string message)  : base(message) 
        {
        }

        public UserAlreadyExistsException(string message, Exception innerException) : base(message, innerException) 
        {
        }
    }
}
