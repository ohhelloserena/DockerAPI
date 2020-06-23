using Microsoft.VisualBasic;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DockerAPI.Services
{
    public class MongoDbService
    {
        private IMongoCollection<TreeNode> _treeNodeCollection { get; set; }
        private string _databaseName = "TradeshiftDatabase";
        private string _collectionName = "start";
        private string _connectionString = "mongodb+srv://tradeshift_user:password_headphones@cluster0-qfjwx.mongodb.net/TradeshiftDatabase? retryWrites =true&w=majority";
        private int _rootId;


        public MongoDbService()
        {
            try
            {
                var mongoClient = new MongoClient(_connectionString);
                var mongoDatabase = mongoClient.GetDatabase(_databaseName);

                _treeNodeCollection = mongoDatabase.GetCollection<TreeNode>(_collectionName);
                _rootId = FindRootId();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        public TreeNode UpdateChildrenById(int id, int[] newChildren)
        {
            var updatedNode = _treeNodeCollection.FindOneAndUpdate(
                   Builders<TreeNode>.Filter.Where(d => d._id == id),
                   Builders<TreeNode>.Update.Set(d => d.children, newChildren),
                   options: new FindOneAndUpdateOptions<TreeNode>
                   {
                       ReturnDocument = ReturnDocument.After
                   });

            return updatedNode;
        }

        public TreeNode UpdateParentNodeById(int id, int newParentId)
        {
            try
            {
                var updatedNode = _treeNodeCollection.FindOneAndUpdate(
                    Builders<TreeNode>.Filter.Where(d => d._id == id),
                    Builders<TreeNode>.Update.Set(d => d.parent, newParentId),
                    options: new FindOneAndUpdateOptions<TreeNode>
                    {
                        ReturnDocument = ReturnDocument.After
                    });

                return updatedNode;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public TreeNode GetTreeNodeById(int? id)
        {
            if (id == null) return null;

            var document = _treeNodeCollection.Find(d => d._id == id);

            return document.FirstOrDefault();
        }

        public int FindDepth(int rootId, int id)
        {
            TreeNode rootNode = GetTreeNodeById(rootId);

            if (rootNode._id == id)
            {
                return 0;
            }
            else if (rootNode.children.Count() == 0)  
            {
                return -1;
            }
            else
            {
                List<int> heights = new List<int>();

                foreach (int childId in rootNode.children)
                {
                    heights.Add(FindDepth(childId, id));
                }

                return heights.Max() + 1;
            }
        }

        private int FindRootId()
        {
            var document = _treeNodeCollection.Find(d => d.parent == null);

            return document.FirstOrDefault()._id;
        }

        public int GetRoot()
        {
            return _rootId;
        }


    }
}
