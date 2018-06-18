using System.Linq;
using System.Web.Mvc;
using PagedList;

using Catalogue.Util;
using Catalogue.Core;
using Catalogue.Interfaces;
using Catalogue.Infrastructure;

namespace Catalogue.Controllers.CRUD
{
    public class PositionController : Controller
    {
        IUnitOfWork unit;

        public PositionController()
        {
            this.unit = new UnitOfWork();
        }
        public PositionController(IUnitOfWork unit)
        {
            this.unit = unit;
        }

        [Authorize(Roles = "admin")]
        public ActionResult AjaxPositionList(int? page)
        {
            int pageNumber = (page ?? 1);

            IPagedList<Position> positions = unit
                .Positions
                .GetPositionsOrderedByName()
                .ToPagedList(pageNumber, Constants.PageSize);

            return PartialView(positions);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            IPagedList<Position> positions = unit
                .Positions
                .GetPositionsOrderedByName()
                .ToPagedList(pageNumber, Constants.PageSize);

            return View(positions);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
                 return HttpNotFound();

            Position position = unit.Positions.Get((int)id);
            if (position == null)
                return HttpNotFound();

            return View(position);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(Position collection)
        {
            try
            {
                unit.Positions.Create(collection);
                unit.Save();

                return RedirectToAction("Index");
            }
            catch { return View(); }
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                 return HttpNotFound();

            Position position = unit.Positions.Get((int)id);
            if (position == null)
                return HttpNotFound();

            return View(position);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id, Position collection)
        {
            try
            {
                unit.Positions.Update(collection);
                unit.Save();

                return RedirectToAction("Index");
            }
            catch { return View(); }
        }

        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return HttpNotFound();

            unit.Positions.Delete((int)id);

            return View("Index");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ActionName("Delete")]
        public ActionResult Delete(int? id, Position collection)
        {
            Position position = new Position();
            try
            {
                if (id == null)
                     return HttpNotFound();

                unit.Positions.Delete((int)id);

                return RedirectToAction("Index");
            }
            catch { return View(); }
        }
    }
}