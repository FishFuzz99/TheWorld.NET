using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace TheWorld.NET.Models
{
    public class WorldUser : IdentityUser
    {
        public DateTime FirstTrip{ get; set; }

    }
}