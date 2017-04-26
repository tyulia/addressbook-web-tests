using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;


namespace WebAddressbookTests
{
    [TestFixture]
    public class ContactInformationTests : AuthTestBase
    {
        [Test]
        public void ContactEditInformationTest()
        {
            int index = 0;
            ContactData fromTable = app.Contacts.GetContactInformationFromTable(index);
            ContactData fromEdit = app.Contacts.GetContactInformationFromEditForm(index);

            Assert.AreEqual(fromTable, fromEdit);
            Assert.AreEqual(fromTable.Address, fromEdit.Address);
            Assert.AreEqual(fromTable.AllEmails, fromEdit.AllEmails);
            Assert.AreEqual(fromTable.AllPhones, fromEdit.AllPhones);
        }

        [Test]
        public void ContactViewInformationTest()
        {
            int index = 0;

            ContactData fromEdit = app.Contacts.GetContactInformationFromEditForm(index);
            ContactData fromView = app.Contacts.GetContactInformationFromViewForm(index);

            Assert.AreEqual(fromEdit.View, fromView.View);
        }
    }
}
