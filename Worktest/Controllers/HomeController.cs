using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Worktest.Models;

namespace Worktest.Controllers
{
    public class HomeController : Controller
    {
        public WorkTestContext db { get; set; }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetData(string limit)
        {
            db = new WorkTestContext();
            List<User> json = new List<User>();
            foreach (var val in db.User)
            {
                json.Add(val);
            }

            return Ok(JsonConvert.SerializeObject(json, Formatting.Indented));
        }

        public async Task<IActionResult> CreateRow(string FIO, DateTime Birth, string Phone)
        {
            if (ModelState.IsValid)
            {
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    ViewUserModel result = JsonConvert.DeserializeObject<ViewUserModel>(await reader.ReadToEndAsync());
                    db = new WorkTestContext();
                    User user = new User()
                    {
                        Id = 0,
                        FIO = result.FIO,
                        Birth = result.Birth,
                        Phone = result.Phone
                    };
                    db.User.Add(user);
                    await db.SaveChangesAsync();
                    return Ok();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        public IActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    var result = JsonConvert.DeserializeObject<DeleteViewModel>(reader.ReadToEnd());
                    id = result.id;
                }
                db = new WorkTestContext();
                db.User.Remove(db.User.Where(x => x.Id == id).FirstOrDefault());
                db.SaveChangesAsync();
                return Ok("success");
            }
            else
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> UpdateData()
        {
            if (ModelState.IsValid)
            {
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    User result = JsonConvert.DeserializeObject<User>(await reader.ReadToEndAsync());
                    db = new WorkTestContext();
                    var id = result.Id;
                    var user = db.User.Where(x => x.Id == id).FirstOrDefault();
                    user.Birth = result.Birth ?? user.Birth;
                    user.FIO = result.FIO ?? user.FIO;
                    user.Phone = result.Phone ?? user.Phone;
                    await db.SaveChangesAsync();
                }
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
