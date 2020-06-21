using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DockerAPI.Services
{
    public class MongoDbService
    {
        private IMongoCollection<TreeNode> _TreeNodeCollection { get; set; }
        private string databaseName = "TradeshiftDatabase";
        private string collectionName = "start";
        private string connectionString = "mongodb+srv://tradeshift_user:password_headphones@cluster0-qfjwx.mongodb.net/TradeshiftDatabase? retryWrites =true&w=majority";

        public MongoDbService()
        {
            try
            {
                var mongoClient = new MongoClient(connectionString);
                var mongoDatabase = mongoClient.GetDatabase(databaseName);

                _TreeNodeCollection = mongoDatabase.GetCollection<TreeNode>(collectionName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        public async Task<TreeNode> UpdateParentNodeById(int id, int newParentId)
        {
            try
            {
                var updatedNode = await _TreeNodeCollection.FindOneAndUpdateAsync(
                    Builders<TreeNode>.Filter.Where(d => d._id == id),
                    Builders<TreeNode>.Update.Set(d => d.parent, newParentId),
                    options: new FindOneAndUpdateOptions<TreeNode>
                    {
                        ReturnDocument = ReturnDocument.After
                    });
                //updatedNode.Height = await GetHeight(id);
                //updatedNode.Root = await GetRootId(id);
                
                return updatedNode;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public async Task<TreeNode> GetTreeNodeById(int id)
        {
            var document = await _TreeNodeCollection.FindAsync(d => d._id == id);

            return document.FirstOrDefault();
        }

        public async Task<int> GetHeight(int id)
        {
            return -1;


        }

        public async Task<int> GetRootId(int id)
        {
            //TreeNode node = await GetTreeNodeById(id);

            //if (node.parent == null) return node._id;

            return -1;

           
        }


    }
}
