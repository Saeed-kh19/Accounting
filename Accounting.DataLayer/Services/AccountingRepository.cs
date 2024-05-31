using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting.ViewModels;
using Accounting.ViewModels.Persons;

namespace Accounting.DataLayer.Services
{
    public class AccountingRepository : IAccountingRepository
    {
        //for dependency injection making it private!
        private Accounting_DBEntities db;

        //Creating a constructor for setting value for (db)
        public AccountingRepository(Accounting_DBEntities context)
        {
            db = context;
        }

        //Getting all Persons
        public List<AccountPersons> GetAllPersons()
        {
            return db.AccountPersons.ToList();
        }

        //Filter person by ID
        public AccountPersons GetPersonById(int id)
        {
            return db.AccountPersons.Find(id);
        }

        //Delete a person by its class!
        public bool DeletePerson(AccountPersons person)
        {
            try
            {
                db.Entry(person).State = EntityState.Deleted;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //Delete a person by its ID :)
        public bool DeletePerson(int personId)
        {
            try
            {
                //at first we find it , then with another overload of this function , we delete it! :)
                AccountPersons person = GetPersonById(personId);
                DeletePerson(person);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //Getting a user and then 
        public bool UpdateCredentials(Users user)
        {
            var local = db.Set<Users>().Local.FirstOrDefault(f => f.UserID == user.UserID);
            if (local != null)
            {
                db.Entry(user).State = EntityState.Detached;
            }
            db.Entry(user).State = EntityState.Modified;
            return true;
        }

        //Filter a person by its ID
        public string GetPersonNameById(int id)
        {
            return db.AccountPersons.Find(id).FullName;
        }

        //Getting a person and then insert it into AccountPersons
        public bool InsertPerson(AccountPersons person)
        {
            try
            {
                db.AccountPersons.Add(person);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdatePerson(AccountPersons person)
        {
            try
            {
                db.Entry(person).State = EntityState.Modified;
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Return a list that filtered by filter paramter!! :))))
        public List<AccountPersons> PersonFilter(string filter)
        {
            return db.AccountPersons.Where(l => l.FullName.Contains(filter) || l.PhoneNumber.Contains(filter) || l.Email.Contains(filter)).ToList();
        }

        //Filter a group of persosn with filter parameter! :)
        public List<ListPersonsViewModel> GetAllPersonsNames(string filter)
        {
            if (filter == "")
            {
                return db.AccountPersons.Select(p => new ListPersonsViewModel()
                {
                    PersonID = p.PersonID,
                    PersonFullName = p.FullName
                }).ToList();
            }
            else
            {
                return db.AccountPersons.Where(p => p.FullName.Contains(filter)).Select(p => new ListPersonsViewModel()
                {
                    PersonID = p.PersonID,
                    PersonFullName = p.FullName
                }).ToList();
            }
        }

        //Adding a transaction
        public bool InsertTransaction(Transactions transaction)
        {
            try
            {
                db.Transactions.Add(transaction);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //Listing all transactions
        public List<Transactions> GetAllTransactions()
        {
            return db.Transactions.ToList();
        }

        //Deleting a transaction with detached method!
        public bool DeleteTransaction(Transactions transactions)
        {
            try
            {
                db.Entry(transactions).State = EntityState.Detached;
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Finding a transaction with its ID then refer to its overload method for deleting it! :)
        public bool DeleteTransaction(int transactionId)
        {
            try
            {
                Transactions transaction = GetTransactionsById(transactionId);
                DeleteTransaction(transaction);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Getting transaction with the Find() method! :)
        public Transactions GetTransactionsById(int id)
        {
            return db.Transactions.Find(id);
        }

        //Find a person with its ID
        public AccountPersons GetPersonId(int id)
        {
            return db.AccountPersons.Find(id);
        }

        public bool UpdateTransaction(Transactions transaction)
        {
            try
            {
                db.Entry(transaction).State = EntityState.Modified;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //Income TypeID = 1 so in where we use it to list income trancations! :))))
        public List<Transactions> GetIncomeTransactions()
        {
            return db.Transactions.Where(t => t.TypeID == 1).ToList();
        }

        //Income TypeID = 2 so in where we use it to list income trancations! :))))
        public List<Transactions> GetOutcomeTransactions()
        {
            return db.Transactions.Where(t => t.TypeID == 2).ToList();
        }

        //Getting all users as a list!
        public List<Users> GetAllUsers()
        {
            return db.Users.ToList();
        }
        
        //Filtering a user by its ID and Find() method
        public Users GetUserById(int id)
        {
            return db.Users.Find(id);
        }

        public List<SettingsViewModel> GetAllUsersNames()
        {
            return db.Users.Select(p => new SettingsViewModel()
            {
                UserID = p.UserID,
                Name = p.Name
            }).ToList();
        }

        //Update a user with its user Class! :)
        public bool UpdateUser(Users user)
        {
            try
            {
                db.Entry(user).State = EntityState.Modified;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //Listing TransactionIDs by personId
        public List<int> GetAllTransactionIdsByPersonId(int personId)
        {
            return db.Transactions.Where(t => t.PersonID == personId).Select(t => t.TransactionID).ToList();
        }
    }
}