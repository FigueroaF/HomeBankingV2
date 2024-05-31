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
        }
    }
}
