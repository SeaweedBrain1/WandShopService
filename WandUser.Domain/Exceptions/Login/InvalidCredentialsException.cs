﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WandUser.Domain.Exceptions.Login;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException() : base("Incorect password or login") { }
}
