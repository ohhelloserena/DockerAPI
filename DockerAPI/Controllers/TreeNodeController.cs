using DockerAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DockerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreeNodeController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetDescendents(int id)
        {
            var mongoDbService = new MongoDbService();

            var descendentNodes = new List<TreeNode>();
            var queue = new Queue<TreeNode>();

            try
            {
                var node = await mongoDbService.GetTreeNodeById(id);

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
                            var childNode = await mongoDbService.GetTreeNodeById(child);

                            if (childNode != null)
                            {
                                queue.Enqueue(childNode);

                                childNode.Height = await mongoDbService.GetHeight(childNode._id);
                                childNode.Root = await mongoDbService.GetRootId(childNode._id);

                                descendentNodes.Add(node);
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
        public async Task<IActionResult> UpdateParentNode([FromQuery]QueryParameters parameters)
        {
            try
            {
                var mongoDbService = new MongoDbService();

                var node = await mongoDbService.GetTreeNodeById(parameters.id);

                if (node == null)
                {
                    return NotFound("id is invalid");
                }
                else
                {
                    int id = parameters.id;
                    int newParentId = parameters.newParentId;

                    //var oldParent = await mongoDbService.GetParent(id);

                    var result = await mongoDbService.UpdateParentNodeById(id, newParentId);
                    result.Height = await mongoDbService.GetHeight(id);
                    result.Root = await mongoDbService.GetRootId(id);


                    //if (result is ok)
                    //{
                    //    var result2 = await mongoDbService.RemoveChild(oldParent, id);
                    //}
                    


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