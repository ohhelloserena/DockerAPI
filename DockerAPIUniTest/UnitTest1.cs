using Microsoft.VisualStudio.TestTools.UnitTesting;
using DockerAPI;
using DockerAPI.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace DockerAPIUniTest
{
    [TestClass]
    public class UnitTest1
    {
        private MongoDbService _mongoDbService;
        public UnitTest1()
        {
            _mongoDbService = new MongoDbService();
        }
        [TestMethod]
        public void TestGetTreeByNullId()
        {
            Assert.AreEqual(null, _mongoDbService.GetTreeNodeById(null));
        }

        [TestMethod]
        public void TestGetTreeByInvalidId()
        {
            Assert.AreEqual(null, _mongoDbService.GetTreeNodeById(999999));
        }

        [TestMethod]
        public void TestGetTreeByValidIdd()
        {
            TreeNode actual = _mongoDbService.GetTreeNodeById(15);

            Assert.AreEqual(15, actual._id);
            Assert.AreEqual(8, actual.parent);
            Assert.AreEqual(0, actual.children.Count());
        }





    }
}
