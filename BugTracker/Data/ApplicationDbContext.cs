﻿using System;
using System.Collections.Generic;
using System.Text;
using BugTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }

        public DbSet<BugTracker.Models.Bug> Bugs { get; set; }

        public DbSet<BugTracker.Models.Comment> Comments { get; set; }
    }
}
