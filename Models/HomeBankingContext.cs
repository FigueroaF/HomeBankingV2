using Microsoft.EntityFrameworkCore;
using System.CodeDom;

namespace HomeBankingV1.Models
{
    public class HomeBankingContext : DbContext
    {
       public HomeBankingContext(DbContextOptions<HomeBankingContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }    
        
        //registro de nueva tabla Account
        public DbSet<Account> Accounts { get; set; }
        
        //registro de una nueva tabla Transactions
        public DbSet<Transaction> Transactions { get; set; }
    }
}
