using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContosoUniverstity.DAL;
using ContosoUniverstity.Models;
using ContosoUniverstity.ViewModel;
namespace ContosoUniverstity.Controllers
{
    public class EnrollmentController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Enrollment
        //public ActionResult Index()
        //{
        //    var enrollments = db.Enrollments.Include(e => e.Course).Include(e => e.Student);
        //    return View(enrollments.ToList());
        //}
        public ActionResult Index(int? id)
        {
            var viewModel = new StudentEnrollmentData();
            viewModel.Students = db.Students.Include(s => s.Enrollments)
                .OrderBy(s => s.LastName);
            if (id != null)
            {
                ViewBag.StudentID = id.Value;
                //需将Course包含进来
                viewModel.Enrollments = db.Enrollments.Where(e => e.StudentID == id.Value).Include(e => e.Course);
            }
            return View(viewModel);
        }
        // GET: Enrollment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // GET: Enrollment/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title");
            ViewBag.StudentID = new SelectList(db.People, "ID", "LastName");
            return View();
        }

        // POST: Enrollment/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EnrollmentID,CourseID,StudentID,Grade")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Enrollments.Add(enrollment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.People, "ID", "LastName", enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollment/Edit/5
        //获得Student的ID
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Enrollment enrollment = db.Enrollments.Find(id);
            var viewModel = new StudentEnrollments();
            var student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentID = id.Value;
            viewModel.Student = student;
            //需将Course包含进来
            //viewModel.Enrollments = db.Enrollments.Where(e => e.Student == viewModel.Student).Include(e => e.Course);
            //viewModel.Enrollments = viewModel.Student.Enrollments;
            viewModel.Enrollments = db.Enrollments.Where(e => e.StudentID == viewModel.Student.ID).Include(e => e.Course);
            //ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
            //ViewBag.StudentID = new SelectList(db.People, "ID", "LastName", enrollment.StudentID);

            return View(viewModel);
        }
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Enrollment enrollment = db.Enrollments.Find(id);
        //    if (enrollment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
        //    ViewBag.StudentID = new SelectList(db.People, "ID", "LastName", enrollment.StudentID);
        //    return View(enrollment);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        //可以进入，并且有Grade的值，但无ID值
        public ActionResult Edit(Student student, IEnumerable<Enrollment> enrollment)
        {
            var viewModel = new StudentEnrollments();
            if (student != null)
            {
                viewModel.Student = db.Students.Find(student.ID);
            }
            if (ModelState.IsValid)
            {
              
                List<Enrollment> viewModelEnrollment = new List<Enrollment>();
                foreach (var item in enrollment)
                {
                    var enrollmen = db.Enrollments.Find(item.EnrollmentID);
                    viewModelEnrollment.Add(enrollmen);
                }

            }
            viewModel.Enrollments = db.Enrollments.Where(e => e.StudentID == viewModel.Student.ID).Include(e => e.Course);

            return View(viewModel);
        }

        // POST: Enrollment/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "EnrollmentID,CourseID,StudentID,Grade")] Enrollment enrollment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(enrollment).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
        //    ViewBag.StudentID = new SelectList(db.People, "ID", "LastName", enrollment.StudentID);
        //    return View(enrollment);
        //}

        // GET: Enrollment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // POST: Enrollment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Enrollment enrollment = db.Enrollments.Find(id);
            db.Enrollments.Remove(enrollment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
