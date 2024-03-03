using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace myAppToDoList.Areas.Identity.Data;

public class SampleUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}

