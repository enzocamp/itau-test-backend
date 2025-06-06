using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itau.Investimentos.Domain.Exceptions
{
    public class DbOperationException : Exception
    {
        public DbOperationException(string message, Exception innerException) :base(message,innerException){        
        }
    }
}
