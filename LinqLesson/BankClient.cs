using System.Collections.Generic;
using System.Linq;

namespace LinqLesson
{
    public class BankClient
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public List<Operation> Operations { get; set; }

        public object ClientInfo()
        {
            return new
            {
                FirstName = FirstName,
                LastName = LastName,
                MiddleName = MiddleName,
                FirstDebit = Operations.First(operation => operation.OperationType == "Debit").Date
            };
        }
    }
}