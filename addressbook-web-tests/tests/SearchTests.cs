using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WebAddressbookTests
{
    [TestFixture]
    public class SearchTests : AuthTestBase
    {
        [Test]
        public void TestSearchGoodData()
        {
            string searhData = "rrrrrrrrrrrrr";

            //Проверка наличия "хороших" данных и создание в случае отсутствия
            int presenceData = 0;
            List<ContactData> contactsBefore = ContactData.GetAll();
            foreach (ContactData contact in contactsBefore)
            {
                if (contact.Lastname.Contains(searhData)
                    || contact.Firstname.Contains(searhData)
                    || contact.Address.Contains(searhData)
                    || contact.AllEmails.Contains(searhData)
                    || contact.AllPhones.Contains(searhData))
                    presenceData++;
            }
            if (presenceData == 0)
            {
                app.Contacts.Create(new ContactData(searhData, "ForSearch"));
            }

            //Проверка счетчика
            app.Contacts.FillSearchString(searhData);
            int searhResultFromForm = app.Contacts.GetNumberOfSearchResults();
            int countContacts = app.Contacts.GetContactCount();

            Assert.LessOrEqual(1, searhResultFromForm);
            Assert.AreEqual(countContacts, searhResultFromForm);

            //Проверка найденных контактов
            int searhResult = 0;
            List<ContactData> contacts = app.Contacts.GetDisplayedContactTable();
            foreach (ContactData contact in contacts)
            {
                if (contact.Lastname.Contains(searhData)
                    || contact.Firstname.Contains(searhData)
                    || contact.Address.Contains(searhData)
                    || contact.AllEmails.Contains(searhData)
                    || contact.AllPhones.Contains(searhData))
                    searhResult++;
            }

            Assert.AreEqual(searhResult, searhResultFromForm);
        }

        [Test]
        public void TestSearchBadData()
        {
            string searhData = "ww";

            //Проверка наличия "плохих" данных и удаление в случае наличия
            int indexCount = 0;
            int presenceData = 0;
            List<ContactData> contactsBefore = ContactData.GetAll();
            foreach (ContactData contact in contactsBefore)
            {
                if (contact.Lastname.Contains(searhData)
                    || contact.Firstname.Contains(searhData)
                    || contact.Address.Contains(searhData)
                    || contact.AllEmails.Contains(searhData)
                    || contact.AllPhones.Contains(searhData))
                {
                    app.Contacts.Remove(presenceData - indexCount);
                    indexCount++;
                }
                presenceData++;
            }

            //Проверка счетчика
            app.Contacts.FillSearchString(searhData);
            int searhResultFromForm = app.Contacts.GetNumberOfSearchResults();
            int countContacts = app.Contacts.GetContactCount();

            Assert.AreEqual(0, searhResultFromForm);
            Assert.AreEqual(countContacts, searhResultFromForm);
        }
    }
}
