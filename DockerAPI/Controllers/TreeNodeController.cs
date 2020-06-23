using DockerAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DockerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreeNodeController : Controller
    {
        [HttpGet]
        public IActionResult GetDescendents(int id)
        {
            var mongoDbService = new MongoDbService();

            var descendentNodes = new List<TreeNode>();
            var queue = new Queue<TreeNode>();

            try
            {
                var node = mongoDbService.GetTreeNodeById(id);

                if (node == null)
                {
                    return NotFound("id is invalid or missing");
                }
                else
                {
                    queue.Enqueue(node);

                    while (queue.Count != 0)
                    {
                        TreeNode curr = queue.Dequeue();

                        foreach (int child in curr.children)
                        {
                            var childNode = mongoDbService.GetTreeNodeById(child);

                            if (childNode != null)
                            {
                                queue.Enqueue(childNode);

                                
                                childNode.Root = mongoDbService.GetRoot();
                                childNode.Depth = mongoDbService.FindDepth(childNode.Root, childNode._id);

                                descendentNodes.Add(childNode);
                            }

                        }
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return Ok(descendentNodes);
        }


        [HttpPatch]
        public IActionResult UpdateParentNode([FromQuery]QueryParameters parameters)
        {
            try
            {
                var mongoDbService = new MongoDbService();

                var node = mongoDbService.GetTreeNodeById(parameters.id);
                var parent = mongoDbService.GetTreeNodeById(parameters.newParentId);

                if (node == null || parent == null)
                {
                    return NotFound("id and/or new parent id are invalid");
                }
                else
                {
                    //update old parent's children
                    int? oldParentId = node.parent;

                    if (oldParentId != null)
                    {
                        var parentNode = mongoDbService.GetTreeNodeById(oldParentId);

                        int[] newChildrenList = parentNode.children.Where(c => c != parameters.id).ToArray();
                        mongoDbService.UpdateChildrenById(oldParentId, newChildrenList);
                    }
                    
                    var result = mongoDbService.UpdateParentNodeById(parameters.id, parameters.newParentId);
                    result.Root = mongoDbService.GetRoot();
                    result.Depth = mongoDbService.FindDepth(result.Root, result._id);
                    
                    return Ok(result);
                }

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

    }
}