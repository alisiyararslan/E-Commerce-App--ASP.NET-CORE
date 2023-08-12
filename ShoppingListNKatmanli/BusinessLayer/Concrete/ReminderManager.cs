using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class ReminderManager : IReminderService
    {
        private readonly IReminderDal _reminderDal;

        public ReminderManager(IReminderDal remainderDal)
        {
            _reminderDal = remainderDal;
        }
        public void Delete(Reminder t)
        {
            _reminderDal.Delete(t);
        }

        public Reminder GetElementById(int id)
        {
            return _reminderDal.GetElementById(id);
        }

        public List<Reminder> GetListAll()
        {
            return _reminderDal.GetListAll();
        }

        public void Insert(Reminder t)
        {
            _reminderDal.Insert(t);
        }

        public void Update(Reminder t)
        {
            _reminderDal.Update(t);
        }
    }
}
