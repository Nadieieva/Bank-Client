using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace LinqLesson
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            List<BankClient> clients;
            using (var reader = new StreamReader("bankClients.json"))
            {
                clients = JsonConvert.DeserializeObject<List<BankClient>>(reader.ReadToEnd());
            }

            double debitSum = 0;
            double creditSum = 0;
            foreach (var client in clients)
            {
                var operations = client.Operations;
                debitSum = debitSum + operations.Where(operation => operation.OperationType == "Debit" && operation.Date.Month == 4).Sum(operation => operation.Amount);
                creditSum = creditSum + operations.Where(operation => operation.OperationType == "Credit" && operation.Date.Month == 4).Sum(operation => operation.Amount);
            }
            Console.WriteLine(String.Format("Sum of operation amount in april - Credit: {0}; Debit: {1}", creditSum, debitSum));
            
            var creditAndDebitSums = new Dictionary<string, double>();
            creditAndDebitSums.Add("Credit", creditSum);
            creditAndDebitSums.Add("Debit", debitSum);

            BankClient maxDebitClient = null;
            BankClient maxCreditClient = null;
            double maxCreditSum = 0;
            double maxDebitSum = 0;
            foreach (var client in clients)
            {
                var operations = client.Operations;
                var clientDebits = operations.Where(operation => operation.OperationType == "Debit").Sum(operation => operation.Amount);
                var clientCredits = operations.Where(operation => operation.OperationType == "Credit").Sum(operation => operation.Amount);
                if (clientCredits > maxCreditSum)
                {
                    maxCreditSum = clientCredits;
                    maxCreditClient = client;
                }
                if (clientDebits > maxDebitSum)
                {
                    maxDebitSum = clientDebits;
                    maxDebitClient = client;
                }
            }
            Console.WriteLine(String.Format("Client with max debit sum {0}", maxDebitClient.ClientInfo()));
            Console.WriteLine(String.Format("Client with max credit sum {0}", maxCreditClient.ClientInfo()));

            var clientsNotCredited = new List<object>();
            Console.WriteLine("Clients with no credit in april:");
            foreach (var client in clients)
            {
                var operations = client.Operations;
                var creditOperations = operations.Where(operation =>
                    operation.Date.Month == 4 && operation.OperationType == "Credit");
                if (!creditOperations.Any())
                {
                    var clientInfo = client.ClientInfo();
                    clientsNotCredited.Add(clientInfo);
                    Console.WriteLine(clientInfo);
                }
            }

            var customObject = new
            {
                CreditAndDebitSums = creditAndDebitSums,
                MaxDebitClient = maxDebitClient.ClientInfo(),
                maxCreditClient = maxCreditClient.ClientInfo(),
                ClientsNotCredited = clientsNotCredited
            };
            using (var writer = new StreamWriter("bankOutput.json"))
            {
                writer.WriteLine(JsonConvert.SerializeObject(customObject));
            }

            Console.WriteLine("Press any key");
            Console.ReadKey();
        }
    }
}