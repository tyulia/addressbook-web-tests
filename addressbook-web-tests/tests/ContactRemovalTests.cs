using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WebAddressbookTests
{
    [TestFixture]
    public class ContactRemovalTests : ContactTestBase
    {
        [Test]
        public void ContactRemovalTest()
        {
            int num = 0;
            if (!app.Contacts.FindContact(num))
            {
                ContactData data = new ContactData("contactDelete", "contactDelete");
                app.Contacts.Create(data);
            }

            List<ContactData> oldContacts = ContactData.GetAll();
            ContactData oldData = oldContacts[num];

            app.Contacts.Remove(oldData);

            Assert.AreEqual(oldContacts.Count - 1, app.Contacts.GetContactCount());

            List<ContactData> newContacts = ContactData.GetAll();
            oldContacts.RemoveAt(num);
            Assert.AreEqual(oldContacts, newContacts);

            foreach (ContactData contact in newContacts)
            {
                Assert.AreNotEqual(contact.Id, oldData.Id);
            }
        }
    }
}
