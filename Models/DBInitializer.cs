namespace HomeBankingV1.Models
{
    public class DBInitializer
    {
        public static void Initialize(HomeBankingContext context)
        {
            if (!context.Clients.Any())
            {
                var clients = new Client[]
                {
                    new Client {FirstName="Eduardo",LastName="Mendoza",Email="edu@gmail.com",Password="123"},
                    new Client {FirstName="Facundo",LastName="Figueroa",Email="facu@gmail.com",Password ="123"},
                    new Client {FirstName="Maria",LastName="Lopez",Email="maria@gmail.com",Password="123"},
                    new Client {FirstName="Pedro",LastName="Gomez",Email="pedro@gmail.com",Password="124"},
                };

                context.Clients.AddRange(clients);
                //guardar los datos en la base
                context.SaveChanges();
            }

            if(!context.Accounts.Any())
            {
                Client facuClient = context.Clients.FirstOrDefault(cl => cl.Email == "facu@gmail.com");
                if (facuClient!= null) 
                {
                    var facuAccounts = new Account[]
                    {
                        new Account{Number="VIN001", CreationDate=DateTime.Now,Balance=100000,ClientId=facuClient.Id},
                        new Account{Number="VIN002", CreationDate=DateTime.Now,Balance=200000,ClientId=facuClient.Id},
                    };

                    context.Accounts.AddRange(facuAccounts);
                    context.SaveChanges();
                }

                
            }
            if (!context.Transactions.Any())
                {
                var account1 = context.Accounts.FirstOrDefault(c => c.Number == "VIN001");
                if (account1 != null)
                {
                    var transactions = new Transaction[]
                    {
                        new Transaction
                        {
                            AccountId= account1.Id,
                            Amount= 10000,
                            Date= DateTime.Now.AddHours(-5),
                            Description= "Transferencia Recibida",
                            Type= TransactionType.CREDIT.ToString()

                        },

                        new Transaction
                        {
                            AccountId= account1.Id,
                            Amount= -2000,
                            Date= DateTime.Now.AddHours(-6),
                            Description= "Compra en tienda mercado libre",
                            Type= TransactionType.DEBIT.ToString()
                        },

                        new Transaction
                        {
                            AccountId= account1.Id,
                            Amount= -3000,
                            Date= DateTime.Now.AddHours(-7),
                            Description= "Compra en Supermercado",
                            Type= TransactionType.DEBIT.ToString()},
                    };

                    foreach (Transaction transaction in transactions)
                    {
                        context.Transactions.Add(transaction);
                    }
                    context.Transactions.AddRange(transactions); 
                    context.SaveChanges();

                }
            
            }
        } 
    }
}
