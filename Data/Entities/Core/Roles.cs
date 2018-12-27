﻿using Base.DAL;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.Core
{
    public static class Roles
    {
        public static string Admin { get => "Admin"; }
        public static string Editor { get => "Editor"; }
        public static string Byuer { get => "Byuer"; }
        public static string Public { get => "Public"; }

        public static IEnumerable<string> GetRolesList
        {
            get => new List<string> { Admin, Editor, Byuer, Public };
        } 
    }
}
