using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using tcp_server.Models;

namespace tcp_server.Data
{
    internal class MessageContext : DbContext
    {
        public DbSet<Message> Messages { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=SYM\\SQLEXPRESS01;Initial Catalog=HttpMessage_data;Integrated Security=True;Pooling=False;Encrypt=False;Trust Server Certificate=True");
        }
    }
}
