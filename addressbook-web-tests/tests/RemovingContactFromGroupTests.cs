using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WebAddressbookTests
{
    [TestFixture]
    public class RemovingContactFromGroupTests : AuthTestBase
    {
        [Test]
        public void TestRemovingContactFromGroup()
        {
            GroupData group = GroupData.GetAll()[0];
            List<ContactData> oldListContact = group.GetContacts();
            ContactData contact;
            if (oldListContact.Count > 0)
            {
                contact = oldListContact.First();
            }
            else
            {
                contact = ContactData.GetAll().First();
                app.Contacts.AddContactToGroup(contact, group);
                oldListContact = group.GetContacts();
            }
            List<GroupData> oldListGroup = contact.GetGroups();

            app.Contacts.RemoveContactFromGroup(contact, group);

            List<ContactData> newListContact = group.GetContacts();
            oldListContact.Remove(contact);
            oldListContact.Sort();
            newListContact.Sort();
            Assert.AreEqual(oldListContact, newListContact);

            List<GroupData> newListGroup = contact.GetGroups();
            oldListGroup.Remove(group);
            oldListGroup.Sort();
            newListGroup.Sort();
            Assert.AreEqual(oldListGroup, newListGroup);
        }
    }
}
