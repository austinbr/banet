using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using banetexam2.Models;
using banetexam2.Factories;

namespace banetexam2.Controllers
{
    public class IdeaController : Controller
    {
        private readonly IdeaFactory ideaFactory;
        public IdeaController(IdeaFactory connection)
        {
            ideaFactory = connection;
        }
        
        [HttpGet]
        [Route("dashboard")]
        public IActionResult dashboard()
        {
            if(HttpContext.Session.GetInt32("currUser") == null || (int)HttpContext.Session.GetInt32("currUser") == 0)
            {
               return RedirectToAction("Index"); 
            }
            else{
                ViewBag.ideas = ideaFactory.GetAllIdeas();
                ViewBag.currUser = (int)HttpContext.Session.GetInt32("currUser");
                ViewBag.currUsername = HttpContext.Session.GetString("currUsername");
                return View("dashboard");
            }
        }

        [HttpGet]
        [Route("idea/{IdeaId}")]
        public IActionResult idea(int IdeaId)
        {
            if(HttpContext.Session.GetInt32("currUser") == null || (int)HttpContext.Session.GetInt32("currUser") == 0)
            {
               return RedirectToAction("Index"); 
            }
            else
            {
                ViewBag.idea = ideaFactory.GetIdeaById(IdeaId);
                return View("Idea");
            }
        }

        [HttpGet]
        [Route("delete/{IdeaId}")]
        public IActionResult delete(int IdeaId)
        {
            if(HttpContext.Session.GetInt32("currUser") == null || (int)HttpContext.Session.GetInt32("currUser") == 0)
            {
               return RedirectToAction("Index"); 
            }
            var idea = ideaFactory.GetIdeaById(IdeaId);
            if ((int)HttpContext.Session.GetInt32("currUser") != idea.CreatedBy)
            {
                return RedirectToAction("dashboard");
            }
            else
            {
                ideaFactory.DeleteIdeaById(IdeaId);
                return RedirectToAction("dashboard");
            }
        }

        [HttpPost]
        [Route("idea")]
        public IActionResult idea(string content)
        {
            if(HttpContext.Session.GetInt32("currUser") == null || (int)HttpContext.Session.GetInt32("currUser") == 0)
            {
               return RedirectToAction("Index"); 
            }
            else
            {
            Idea newIdea = new Idea {
                Content = content,
                CreatedBy = (int)HttpContext.Session.GetInt32("currUser"),
            };

            ideaFactory.CreateIdea(newIdea);

            return RedirectToAction("dashboard");
            }
        }

        [HttpGet]
        [Route("like/{IdeaId}")]
        public IActionResult like(int IdeaId)
        {
            if(HttpContext.Session.GetInt32("currUser") == null || (int)HttpContext.Session.GetInt32("currUser") == 0)
            {
               return RedirectToAction("Index"); 
            }
            else
            {
                if(ideaFactory.GetSpecificLike((int)HttpContext.Session.GetInt32("currUser"), IdeaId).Count > 0)
                {
                    return RedirectToAction("dashboard");
                }
                else
                {
                    Like newLike = new Like {
                        Liked = IdeaId,
                        Liker = (int)HttpContext.Session.GetInt32("currUser"),
                    };
                    ideaFactory.CreateLike(newLike);
                    return RedirectToAction("dashboard");
                }
            }
        }
    }
}
