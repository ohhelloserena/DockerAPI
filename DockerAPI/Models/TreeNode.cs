using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DockerAPI
{
    public class TreeNode
    {
        public int _id { get; set; }

        public int? parent { get; set; }

        public int[] children { get; set; }

        public int Root { get; set; }

        public int Height { get; set; }
    }
}
