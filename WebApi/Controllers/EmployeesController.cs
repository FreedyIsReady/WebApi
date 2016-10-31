using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDataAccess;
using AutoMapper;


namespace WebApi.Controllers
{
    public class EmployeesController : ApiController
    {
        private readonly EmployeeDbContext db;

        public EmployeesController()
        {
            this.db = new EmployeeDbContext();
        }


        public HttpResponseMessage Get(string gender = "All")
        {

            switch (gender.ToUpper())
            {
                case "ALL":
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, db.Employees.ToList());
                    }
                case "MALE":
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, db.Employees.
                            Where(x => x.Gender.ToUpper() == "MALE").ToList());
                    }
                case "FEMALE":
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, db.Employees.
                            Where(x => x.Gender.ToUpper() == "FEMALE").ToList());
                    }
                default:
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                        "gender must be ALL, MALE or FEMALE");

            }
        }


        public HttpResponseMessage Get(int id)
        {
            var employee = db.Employees.Find(id);

            if (employee == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Employee with id=" + id + " not exists.");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, employee);
            }
        }

        public HttpResponseMessage Post([FromBody]Employee employee)
        {
            try
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.Created, employee);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Put(int id, [FromBody] Employee employee)
        {
            try
            {
                var newEmployee = db.Employees.Find(id);

                if (newEmployee != null)
                {

                    newEmployee.FirstName = employee.FirstName;
                    newEmployee.LastName = employee.LastName;
                    newEmployee.Gender = employee.Gender;
                    newEmployee.Salary = employee.Salary;
                    db.Entry(newEmployee).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, newEmployee);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id=" + id + " not found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        public HttpResponseMessage Delete(int id)
        {
            var employee = db.Employees.Find(id);

            if (employee != null)
            {
                db.Employees.Remove(employee);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, employee);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Employee with id=" + id + "not found");
            }
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }



    }
}
