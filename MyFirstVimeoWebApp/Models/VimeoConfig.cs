using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstVimeoWebApp.Models
{
    public interface IVimeoConfig
    {
        decimal AppUserId { get; }
    }

    public class VimeoConfig : IVimeoConfig
    {
        public decimal AppUserId { get; set; }
    }
}
