using System;
using System.Collections.Generic;
using System.Text;

namespace MicrShopping.Domain.Infrastructure
{
    public class RabbitMQConfig
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
