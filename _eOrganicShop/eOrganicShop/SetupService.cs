using eOrganicShop.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eOrganicShop
{
    public class SetupService
    {
        public static void Init(eOrganicShopContext context)
        {
            context.Database.Migrate();
        }
    }
}
