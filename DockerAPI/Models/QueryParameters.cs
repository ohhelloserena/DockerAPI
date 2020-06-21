using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DockerAPI
{
    public class QueryParameters
    {
        [BindRequired]
        public int id { get; set; }

        [BindRequired]
        public int newParentId { get; set; }
    }
}
