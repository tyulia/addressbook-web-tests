using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Text.RegularExpressions;

namespace WebAddressbookTests
{
    public class ContactHelper : HelperBase
    {
        public ContactHelper(ApplicationManager manager) : base(manager) { }

        public ContactHelper Create(ContactData contact)
        {
            manager.Navigator.GoToAddContactsPage();
            FillContactForm(contact);
            SubmitContactCreation();
            ReturnToHomePage();
            return this;
        }

        public ContactHelper Modify(int v, ContactData newData)
        {
            manager.Navigator.GoToHomePage();
            SelectContactModification(v);
            FillContactFormWithoutGroup(newData);
            SubmitContactModification();
            ReturnToHomePage();
            return this;
        }

        public ContactHelper Modify(ContactData oldData, ContactData newData)
        {
            manager.Navigator.GoToHomePage();
            SelectContactModification(oldData.Id);
            FillContactFormWithoutGroup(newData);
            SubmitContactModification();
            ReturnToHomePage();
            return this;
        }

        public ContactHelper Remove(int v)
        {
            manager.Navigator.GoToHomePage();
            SelectContact(v);
            SubmitRemove();
            ConfirmRemove();
            manager.Navigator.GoToHomePage();
            return this;
        }

        public ContactHelper Remove(ContactData contact)
        {
            manager.Navigator.GoToHomePage();
            SelectContact(contact.Id);
            SubmitRemove();
            ConfirmRemove();
            manager.Navigator.GoToHomePage();
            return this;
        }

        public ContactHelper AddContactToGroup(ContactData contact, GroupData group)
        {
            manager.Navigator.GoToHomePage();
            ClearGroupFilter();
            SelectContact(contact.Id);
            SelectGroupToAdd(group.Name);
            CommitAddingContactToGroup();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.FindElements(By.CssSelector("div.msgbox")).Count > 0);
            return this;
        }

        public ContactHelper RemoveContactFromGroup(ContactData contact, GroupData group)
        {
            manager.Navigator.GoToHomePage();
            SelectGroupFilter(group);
            SelectContact(contact.Id);
            CommitRemovingContactFromGroup();
            ReturnToGroupNamePage(group);
            ClearGroupFilter();
            return this;
        }

        public ContactHelper ReturnToGroupNamePage(GroupData group)
        {
            driver.FindElement(By.LinkText("group page \"" + group.Name + "\"")).Click();
            return this;
        }

        public ContactHelper CommitAddingContactToGroup()
        {
            driver.FindElement(By.Name("add")).Click();
            return this;
        }

        public ContactHelper CommitRemovingContactFromGroup()
        {
            driver.FindElement(By.Name("remove")).Click();
            return this;
        }

        public ContactHelper SelectGroupToAdd(string name)
        {
            new SelectElement(driver.FindElement(By.Name("to_group"))).SelectByText(name);
            return this;
        }

        public ContactHelper ClearGroupFilter()
        {
            new SelectElement(driver.FindElement(By.Name("group"))).SelectByText("[all]");
            return this;
        }

        public ContactHelper SelectGroupFilter(GroupData group)
        {
            new SelectElement(driver.FindElement(By.Name("group"))).SelectByText(group.Name);
            return this;
        }

        public bool FindContact(int v)
        {
            manager.Navigator.GoToHomePage();
            if (IsElementPresent(By.XPath("//table[@id='maintable']/tbody/tr[" + (v + 2) + "]/td/input")))
            {
                return true;
            }
            return false;
        }

        public ContactHelper SelectContact(int v)
        {
            driver.FindElement(By.XPath("//table[@id='maintable']/tbody/tr[" + (v + 2) + "]/td/input")).Click();
            return this;
        }

        public ContactHelper SelectContact(string contactId)
        {
            driver.FindElement(By.Id(contactId)).Click();
            return this;
        }

        public ContactHelper SelectContactModification(int v)
        {
            driver.FindElement(By.XPath("(//img[@alt='Edit'])[" + (v + 1) + "]")).Click();
            //driver.FindElements(By.Name("entry"))[v]
            //    .FindElements(By.TagName("td"))[7]
            //    .FindElement(By.TagName("a")).Click();
            return this;
        }

        public ContactHelper SelectContactModification(string id)
        {
            IList<IWebElement> lines = driver.FindElements(By.Name("entry"));
            foreach (IWebElement line in lines)
            {
                string idForm = line.FindElement(By.XPath("td[1]")).FindElement(By.TagName("input")).GetAttribute("id");
                if (idForm == id)
                {
                    line.FindElement(By.XPath("td[8]")).Click();
                    break;
                }
            }
            return this;
        }

        public ContactHelper SelectContactView(int v)
        {
            driver.FindElement(By.XPath("(//img[@alt='Details'])[" + (v + 1) + "]")).Click();
            return this;
        }

        public ContactHelper FillContactForm(ContactData contact)
        {
            FillContactFormWithoutGroup(contact);
            TypeSelect(By.Name("new_group"), contact.Newgroup);
            return this;
        }

        public ContactHelper FillContactFormWithoutGroup(ContactData contact)
        {
            Type(By.Name("firstname"), contact.Firstname);
            Type(By.Name("middlename"), contact.Middlename);
            Type(By.Name("lastname"), contact.Lastname);
            Type(By.Name("nickname"), contact.Nickname);
            Type(By.Name("photo"), contact.Photo);
            Type(By.Name("title"), contact.Title);
            Type(By.Name("company"), contact.Company);
            Type(By.Name("address"), contact.Address);
            Type(By.Name("home"), contact.HomePhone);
            Type(By.Name("mobile"), contact.MobilePhone);
            Type(By.Name("work"), contact.WorkPhone);
            Type(By.Name("fax"), contact.Fax);
            Type(By.Name("email"), contact.Email);
            Type(By.Name("email2"), contact.Email2);
            Type(By.Name("email3"), contact.Email3);
            Type(By.Name("homepage"), contact.Homepage);
            TypeSelect(By.Name("bday"), contact.Bday);
            TypeSelect(By.Name("bmonth"), contact.Bmonth);
            Type(By.Name("byear"), contact.Byear);
            TypeSelect(By.Name("aday"), contact.Aday);
            TypeSelect(By.Name("amonth"), contact.Amonth);
            Type(By.Name("ayear"), contact.Ayear);
            Type(By.Name("address2"), contact.Address2);
            Type(By.Name("phone2"), contact.Phone2);
            Type(By.Name("notes"), contact.Notes);
            return this;
        }

        public ContactHelper SubmitContactCreation()
        {
            driver.FindElement(By.XPath("(//input[@name='submit'])[2]")).Click();
            contactCache = null;
            return this;
        }

        public ContactHelper SubmitContactModification()
        {
            driver.FindElement(By.XPath("(//input[@name='update'])[2]")).Click();
            contactCache = null;
            return this;
        }

        public ContactHelper SubmitRemove()
        {
            driver.FindElement(By.XPath("//input[@value='Delete']")).Click();
            contactCache = null;
            return this;
        }

        public ContactHelper ConfirmRemove()
        {
            driver.SwitchTo().Alert().Accept();
            return this;
        }

        public ContactHelper ReturnToHomePage()
        {
            driver.FindElement(By.LinkText("home page")).Click();
            return this;
        }

        private List<ContactData> contactCache = null;

        public List<ContactData> GetContactList()
        {
            if (contactCache == null)
            {
                contactCache = new List<ContactData>();
                manager.Navigator.GoToHomePage();
                ICollection<IWebElement> elements = driver.FindElements(By.XPath("//*[@name='entry']"));
                foreach (IWebElement element in elements)
                {
                    contactCache.Add(new ContactData(element.FindElement(By.XPath("td[3]")).Text,
                        element.FindElement(By.XPath("td[2]")).Text)
                    {
                        Id = element.FindElement(By.XPath("td[1]")).FindElement(By.TagName("input")).GetAttribute("value")
                    });
                }
            }
            return new List<ContactData>(contactCache);
        }

        public int GetContactCount()
        {
            int count = 0;
            ICollection<IWebElement> lines = driver.FindElements(By.XPath("//*[@name='entry']"));
            foreach (IWebElement line in lines)
            {
                if (line.Displayed)
                    count++;
            }
            return count;
        }

        public ContactData GetContactInformationFromTable(int index)
        {
            manager.Navigator.GoToHomePage();
            IList<IWebElement> cells = driver.FindElements(By.Name("entry"))[index].FindElements(By.TagName("td"));
            string lastname = cells[1].Text;
            string firstName = cells[2].Text;
            string address = cells[3].Text;

            string allEmails = cells[4].Text;
            string allPhones = cells[5].Text;

            return new ContactData(firstName, lastname)
            {
                Address = address,
                AllEmails = allEmails,
                AllPhones = allPhones
            };
        }

        public ContactData GetContactInformationFromEditForm(int index)
        {
            manager.Navigator.GoToHomePage();
            SelectContactModification(index);

            string firstName = driver.FindElement(By.Name("firstname")).GetAttribute("value");
            string middleName = driver.FindElement(By.Name("middlename")).GetAttribute("value");
            string lastname = driver.FindElement(By.Name("lastname")).GetAttribute("value");
            string nickName = driver.FindElement(By.Name("nickname")).GetAttribute("value");

            string title = driver.FindElement(By.Name("title")).GetAttribute("value");
            string company = driver.FindElement(By.Name("company")).GetAttribute("value");
            string address = driver.FindElement(By.Name("address")).GetAttribute("value");

            string homePhone = driver.FindElement(By.Name("home")).GetAttribute("value");
            string mobilePhone = driver.FindElement(By.Name("mobile")).GetAttribute("value");
            string workPhone = driver.FindElement(By.Name("work")).GetAttribute("value");

            string fax = driver.FindElement(By.Name("fax")).GetAttribute("value");

            string email = driver.FindElement(By.Name("email")).GetAttribute("value");
            string email2 = driver.FindElement(By.Name("email2")).GetAttribute("value");
            string email3 = driver.FindElement(By.Name("email3")).GetAttribute("value");
            string homepage = driver.FindElement(By.Name("homepage")).GetAttribute("value");

            string bday = driver.FindElement(By.Name("bday")).FindElement(By.XPath("option[1]")).Text;
            string bmonth = driver.FindElement(By.Name("bmonth")).FindElement(By.XPath("option[1]")).Text;
            string byear = driver.FindElement(By.Name("byear")).GetAttribute("value");

            string aday = driver.FindElement(By.Name("aday")).FindElement(By.XPath("option[1]")).Text;
            string amonth = driver.FindElement(By.Name("amonth")).FindElement(By.XPath("option[1]")).Text;
            string ayear = driver.FindElement(By.Name("ayear")).GetAttribute("value");

            string address2 = driver.FindElement(By.Name("address2")).GetAttribute("value");
            string phone2 = driver.FindElement(By.Name("phone2")).GetAttribute("value");
            string notes = driver.FindElement(By.Name("notes")).GetAttribute("value");

            return new ContactData(firstName, lastname)
            {
                Firstname = firstName,
                Middlename = middleName,
                Lastname = lastname,
                Nickname = nickName,
                Title = title,
                Company = company,
                Address = address,
                HomePhone = homePhone,
                MobilePhone = mobilePhone,
                WorkPhone = workPhone,
                Fax = fax,
                Email = email,
                Email2 = email2,
                Email3 = email3,
                Homepage = homepage,
                Bday = bday,
                Bmonth = bmonth,
                Byear = byear,
                Aday = aday,
                Amonth = amonth,
                Ayear = ayear,
                Address2 = address2,
                Phone2 = phone2,
                Notes = notes
            };
        }

        public ContactData GetContactInformationFromViewForm(int index)
        {
            manager.Navigator.GoToHomePage();
            SelectContactView(index);
            string data = GetViewData();
            return new ContactData(null, null)
            {
                View = data
            };
        }

        private string GetViewData()
        {
            return driver.FindElement(By.Id("content")).Text;
        }

        public int GetNumberOfSearchResults()
        {
            manager.Navigator.GoToHomePage();
            //string text = driver.FindElement(By.TagName("label")).Text;
            //Match m = new Regex(@"\d+").Match(text);
            //return Int32.Parse(m.Value);
            string text = driver.FindElement(By.TagName("label")).FindElement(By.Id("search_count")).Text;
            return Int32.Parse(text);
        }

        public void FillSearchString(string text)
        {
            manager.Navigator.GoToHomePage();
            Type(By.Name("searchstring"), text);
        }

        public List<ContactData> GetDisplayedContactTable()
        {
            List<ContactData> contacts = new List<ContactData>();
            IList<IWebElement> lines = driver.FindElements(By.XPath("//*[@name='entry']"));
            int index = 0;
            foreach (IWebElement line in lines)
            {
                if (line.Displayed)
                {
                    contacts.Add(GetContactInformationFromTable(index));
                }
                index++;
            }
            return contacts;
        }
    }
}
