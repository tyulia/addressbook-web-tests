using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebAddressbookTests
{
    [Table(Name = "addressbook")]
    public class ContactData : IEquatable<ContactData>, IComparable<ContactData>
    {
        private string allPhones;
        private string allEmails;
        private string view;

        public ContactData()
        {
        }

        public ContactData(string firstname, string lastname)
        {
            Firstname = firstname;
            Lastname = lastname;
        }

        public bool Equals(ContactData other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                return false;
            }
            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }
            return Firstname == other.Firstname && Lastname == other.Lastname;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Firstname.GetHashCode(), Lastname.GetHashCode()).GetHashCode();
        }

        public override string ToString()
        {
            return "Firstname = " + Firstname + ", Lastname = " + Lastname;
        }

        public int CompareTo(ContactData other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                return 1;
            }
            if (Lastname.CompareTo(other.Lastname) != 0)
            {
                return Lastname.CompareTo(other.Lastname);
            }
            return Firstname.CompareTo(other.Firstname);
        }

        [Column(Name = "firstname")]
        public string Firstname { get; set; }

        [Column(Name = "middlename")]
        public string Middlename { get; set; }

        [Column(Name = "lastname")]
        public string Lastname { get; set; }

        [Column(Name = "nickname")]
        public string Nickname { get; set; }

        [Column(Name = "photo")]
        public string Photo { get; set; }

        [Column(Name = "title")]
        public string Title { get; set; }

        [Column(Name = "company")]
        public string Company { get; set; }

        [Column(Name = "address")]
        public string Address { get; set; }

        [Column(Name = "home")]
        public string HomePhone { get; set; }

        [Column(Name = "mobile")]
        public string MobilePhone { get; set; }

        [Column(Name = "work")]
        public string WorkPhone { get; set; }

        [Column(Name = "fax")]
        public string Fax { get; set; }

        [Column(Name = "email")]
        public string Email { get; set; }

        [Column(Name = "email2")]
        public string Email2 { get; set; }

        [Column(Name = "email3")]
        public string Email3 { get; set; }

        [Column(Name = "homepage")]
        public string Homepage { get; set; }

        [Column(Name = "bday")]
        public string Bday { get; set; }

        [Column(Name = "bmonth")]
        public string Bmonth { get; set; }

        [Column(Name = "byear")]
        public string Byear { get; set; }

        [Column(Name = "aday")]
        public string Aday { get; set; }

        [Column(Name = "amonth")]
        public string Amonth { get; set; }

        [Column(Name = "ayear")]
        public string Ayear { get; set; }

        public string Newgroup { get; set; }

        [Column(Name = "address2")]
        public string Address2 { get; set; }

        [Column(Name = "phone2")]
        public string Phone2 { get; set; }

        [Column(Name = "notes")]
        public string Notes { get; set; }

        [Column(Name = "id"), PrimaryKey]
        public string Id { get; set; }

        [Column(Name = "deprecated")]
        public string Deprecated { get; set; }

        public static List<ContactData> GetAll()
        {
            using (AddressBookDB db = new AddressBookDB())
            {
                return (from c in db.Contacts.Where(x => x.Deprecated == "0000-00-00 00:00:00")
                        select c).ToList();
            }
        }

        public List<GroupData> GetGroups()
        {
            using (AddressBookDB db = new AddressBookDB())
            {
                return (from g in db.Groups
                        from gcr in db.GCR.Where(p => p.ContactId == Id && p.GroupId == g.Id)
                        select g).Distinct().ToList();
            }
        }

        public string AllPhones
        {
            get
            {
                if (allPhones != null)
                {
                    return allPhones;
                }
                else
                {
                    return (CleanUpPhone(HomePhone) + CleanUpPhone(MobilePhone) + CleanUpPhone(WorkPhone) + CleanUpPhone(Phone2)).Trim();
                }
            }
            set
            {
                allPhones = value;
            }
        }

        private string CleanUpPhone(string phone)
        {
            if (phone == null || phone == "")
            {
                return "";
            }
            return Regex.Replace(phone, @"[ \-()]", "") + "\r\n";
        }

        public string AllEmails
        {
            get
            {
                if (allEmails != null)
                {
                    return allEmails;
                }
                else
                {
                    return (CleanUpEmail(Email) + CleanUpEmail(Email2) + CleanUpEmail(Email3)).Trim();
                }
            }
            set
            {
                allEmails = value;
            }
        }

        private string CleanUpEmail(string email)
        {
            if (email == null || email == "")
            {
                return "";
            }
            return email + "\r\n";
        }

        public string View
        {
            get
            {
                if (view != null)
                {
                    return view;
                }
                else
                {
                    return (ConstructionNameBlock() 
                        + ConstructionPhoneBlock() 
                        + ConstructionEmailBlock() 
                        + ConstructionDateBlock() 
                        + ConstructionSecondaryBlock()).Trim();
                }
            }
            set
            {
                view = value;
            }
        }

        private string ConstructionNameBlock()
        {
            string namestr = "";
            if (!String.IsNullOrEmpty(Firstname) || !String.IsNullOrEmpty(Middlename) || !String.IsNullOrEmpty(Lastname))
            {
                string name = "";
                if (!String.IsNullOrEmpty(Firstname))
                    name += Firstname + " ";
                if (!String.IsNullOrEmpty(Middlename))
                    name += Middlename + " ";
                if (!String.IsNullOrEmpty(Lastname))
                    name += Lastname;
                namestr = name.Trim() + "\r\n";
            }

            if (!String.IsNullOrEmpty(Nickname))
                namestr += Nickname + "\r\n";

            if (!String.IsNullOrEmpty(Title))
                namestr += Title + "\r\n";

            if (!String.IsNullOrEmpty(Company))
                namestr += Company + "\r\n";

            if (!String.IsNullOrEmpty(Address))
                namestr += Address + "\r\n";

            if(namestr != "")
                return namestr + "\r\n";
            return namestr;
        }

        private string ConstructionPhoneBlock()
        {
            string phonestr = "";

            if (!String.IsNullOrEmpty(HomePhone))
                phonestr += "H: " + HomePhone + "\r\n";

            if (!String.IsNullOrEmpty(MobilePhone))
                phonestr += "M: " + MobilePhone + "\r\n";

            if (!String.IsNullOrEmpty(WorkPhone))
                phonestr += "W: " + WorkPhone + "\r\n";

            if (!String.IsNullOrEmpty(Fax))
                phonestr += "F: " + Fax + "\r\n";

            if (phonestr != "")
                return phonestr + "\r\n";
            return phonestr;
        }

        private string ConstructionEmailBlock()
        {
            string emailstr = "";

            if (!String.IsNullOrEmpty(Email))
                emailstr += ConstructionEmailString(Email);

            if (!String.IsNullOrEmpty(Email2))
                emailstr += ConstructionEmailString(Email2);

            if (!String.IsNullOrEmpty(Email3))
                emailstr += ConstructionEmailString(Email3);

            if (!String.IsNullOrEmpty(Homepage))
                emailstr += "Homepage:\r\n" + Homepage + "\r\n";

            if (emailstr != "")
                return emailstr + "\r\n";
            return emailstr;
        }

        private string ConstructionDateBlock()
        {
            string datestr = "";

            if ((!String.IsNullOrEmpty(Bday) && Bday != "-") || (!String.IsNullOrEmpty(Bmonth) && Bmonth != "-") || !String.IsNullOrEmpty(Byear))
            {
                string dateData = "";
                dateData += "Birthday ";
                if (!String.IsNullOrEmpty(Bday) && Bday!= "-")
                    dateData += Bday + ". ";
                if (!String.IsNullOrEmpty(Bmonth) && Bmonth != "-")
                    dateData += Bmonth + " ";
                if (!String.IsNullOrEmpty(Byear))
                {
                    dateData += Byear + " (" + CalculateYears(Bday, Bmonth, Byear) + ")";
                }
                datestr += dateData.Trim() + "\r\n";
            }

            if ((!String.IsNullOrEmpty(Aday) && Aday != "-") || (!String.IsNullOrEmpty(Amonth) && Amonth != "-" )|| !String.IsNullOrEmpty(Ayear))
            {
                string dateData = "";
                dateData += "Anniversary ";
                if (!String.IsNullOrEmpty(Aday) && Aday != "-")
                    dateData += Aday + ". ";
                if (!String.IsNullOrEmpty(Amonth) && Amonth != "-")
                    dateData += Amonth + " ";
                if (!String.IsNullOrEmpty(Ayear))
                {
                    dateData += Ayear + " (" + CalculateYears(Aday, Amonth, Ayear) + ")";
                }
                datestr += dateData.Trim() + "\r\n";
            }

            if (datestr != "")
                return datestr + "\r\n";
            return datestr;
        }

        private string ConstructionSecondaryBlock()
        {
            string secondarystr = "";

            if (!String.IsNullOrEmpty(Address2))
                secondarystr += Address2 + "\r\n\r\n";

            if (!String.IsNullOrEmpty(Phone2))
                secondarystr += "P: " + Phone2 + "\r\n\r\n";

            if (!String.IsNullOrEmpty(Notes))
                secondarystr += Notes;

            return secondarystr.Trim();
        }

        private int CalculateYears(string day, string month, string year)
        {
            if (String.IsNullOrEmpty(day) || day == "-")
                day = "1";
            if (String.IsNullOrEmpty(month) || month == "-")
                month = "January";
            DateTime dt = DateTime.ParseExact(day + " " + month + " " + year, "d MMMM yyyy", CultureInfo.InvariantCulture);
            int YearsPassed = DateTime.Now.Year - dt.Year;
            if (DateTime.Now.Month < dt.Month || (DateTime.Now.Month == dt.Month && DateTime.Now.Day < dt.Day))
            {
                YearsPassed--;
            }
            return YearsPassed;
        }

        private string ConstructionEmailString(string email)
        {
            if (new Regex("@").Matches(email).Count == 1)
            {
                if(email.Split('@')[1] != "")
                    return email + " (www." + email.Split('@')[1] + ")\r\n";
                return email + " (ERROR e-mail) \r\n";
            }
            else
                return email + " (ERROR e-mail) \r\n";
        }
    }
}
