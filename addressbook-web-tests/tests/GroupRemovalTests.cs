using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections.Generic;
using NUnit.Framework;

namespace WebAddressbookTests
{
    [TestFixture]
    public class GroupRemovalTests : GroupTestBase
    {
        [Test]
        public void GroupRemovalTest()
        {
            int num = 0;
            if (!app.Groups.FindGroup(num))
            {
                GroupData data = new GroupData("groupDelete");
                app.Groups.Create(data);
            }

            List<GroupData> oldGroups = GroupData.GetAll();//app.Groups.GetGroupList();
            GroupData oldData = oldGroups[num];

            app.Groups.Remove(oldData);

            Assert.AreEqual(oldGroups.Count - 1, app.Groups.GetGroupCount());

            List<GroupData> newGroups = GroupData.GetAll();//app.Groups.GetGroupList();
            oldGroups.RemoveAt(num);
            Assert.AreEqual(oldGroups, newGroups);

            foreach (GroupData group in newGroups)
            {
                Assert.AreNotEqual(group.Id, oldData.Id);
            }
        }
    }
}
