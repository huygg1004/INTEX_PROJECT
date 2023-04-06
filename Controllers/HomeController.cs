using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Intex_app.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Intex_app.Models.ViewModels;
using Intex_app.Services;
using System.IO;
using Intex_app.Infrastructure;
using System.Xml;
using System.Text;
using System.Net;
using com.sun.org.apache.xml.@internal.resolver.helpers;
using System.Security.Claims;

namespace Intex_app.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //public Token _token { get; set; }
        private GamousContext _context { get; set; }
        private PhotoContext _contextphoto { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly S3interface _s3; 
        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, GamousContext context,S3interface s3, PhotoContext contextphoto)//, Token token)
        {
            _logger = logger;
            _userManager = userManager;
            //_token = token;
            _context = context;
            _s3 = s3;
            _contextphoto = contextphoto;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult arcgis()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PhotoSave(Photo savedPhoto)
        {
            if (ModelState.IsValid)
            {
                string url = await _s3.AddItem(savedPhoto.uploadphoto, "testing");

                savedPhoto.photoUrl = url;

                Photo newPhoto = new Photo()
                {
                    PhotoID = savedPhoto.PhotoID,
                    BurialSiteID = savedPhoto.BurialSiteID,
                    photoUrl = savedPhoto.photoUrl
                };

                _contextphoto.Add(newPhoto);
                _contextphoto.SaveChanges();

                ViewBag.Url = url;

                List<Photo> listPhoto = new List<Photo>();
                listPhoto = _contextphoto.Photos.FromSqlRaw("SELECT * FROM Photos;").ToList();

                return View("ViewPhotos",listPhoto);
            }
            else
            {
                return View("PhotoUploadForm");
            }
        }

        public IActionResult PhotoUploadForm()
        {
            return View();
        }

        public IActionResult ViewPhotos()
        {
            return View(_contextphoto);
        }

       

        //[HttpPost]
        //public async Task<IActionResult> PhotoUploadForm(StorageUploadForm file)
        //{
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        await file.photofile.CopyToAsync(memoryStream);

        //        await StorageUpload.UploadFileAsync(memoryStream, "YOUR_BUCKET_NAME", "YOUR_KEY_NAME");
        //    }

        //    return View();
        //}

        //public IActionResult ViewBurialsPublic(long? id)
        //{
        //    //return View();
        //    return View(_context.LocationMeasurements
        //        .FromSqlInterpolated($"SELECT * FROM LocationMeasurement WHERE Id = {id} or {id} IS NULL")
        //        .ToList());

        //    //return View(_context.LocationMeasurements);
        //}

        #region Public View
        public IActionResult ViewBurialsPublic()
        {
            var request = (HttpWebRequest)WebRequest.Create("https://10ay.online.tableau.com/api/3.11/auth/signin");
            request.Method = "POST";
            request.ContentType = "text/xml";

            byte[] bytes;
            bytes = Encoding.ASCII.GetBytes("<tsRequest><credentials personalAccessTokenName = 'demo' personalAccessTokenSecret = 'uwl6pad4SkCd83VdVkmUAg==:t2SDxSqKBK0rcbEmpYe2TnSTpSFQLC8W'><site contentUrl = 'Intex'/></credentials></tsRequest>");

            request.ContentLength = bytes.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();

            var result = (HttpWebResponse)request.GetResponse();
            var user = "";

            //Console.WriteLine(result);


            using (Stream stream = result.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                String responseString = reader.ReadToEnd();

                XmlDocument docu = new XmlDocument();

                docu.LoadXml(responseString);

                var tableauTokenString = docu.GetElementsByTagName("credentials")[0].Attributes[0].Value;
                user = docu.GetElementsByTagName("credentials")[0].ChildNodes[1].Attributes[0].Value;
                Token tableauToken = new Token();

                tableauToken.TokenString = tableauTokenString.ToString();

                HttpContext.Session.SetJson("TableauToken", tableauToken);
            }

            SessionToken TableauToken = HttpContext.Session.GetJason<SessionToken>("TableauToken")
                            ?? new SessionToken();

            if (TableauToken.TokenString != null)
            {
                return View("ViewBurialsPublic", user);
                //return Redirect("https://10ay.online.tableau.com/#/site/intex/workbooks/820859?:origin=card_share_link");
            }
            else
            {
                return View();
            }
        }
        public IActionResult OsteologyPublic(string? id, int pageNum = 1)
        {
            //return View(context.Recipes
            //    .FromSqlInterpolated($"SELECT * FROM Recipes WHERE RecipeClassId = {mealtypeid} OR {mealtypeid} IS NULL")
            //    .ToList());

            int pageSize = 50;

            return View(new IndexViewModel
            {
                Osteologies = (_context.Osteologies
                .Where(m => m.Id == id || id == null)
                .OrderBy(m => m.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList()),

                PageNumberingInfo = new PageNumberingInfo
                {
                    NumItemsPerPage = pageSize,
                    CurrentPage = pageNum,

                    //if no team has been selected, then get the full count. Otherwise only count the number from the team name that has been selected
                    TotalNumItems = (id == null ? _context.Osteologies.Count() :
                     _context.Osteologies.Where(x => x.Id == id).Count())
                },


            });
        }
        public IActionResult OsteologySkullPublic(string? id, int pageNum = 1)
        {
            //return View(context.Recipes
            //    .FromSqlInterpolated($"SELECT * FROM Recipes WHERE RecipeClassId = {mealtypeid} OR {mealtypeid} IS NULL")
            //    .ToList());

            int pageSize = 50;

            return View(new IndexViewModel
            {
                OsteologySkulls = (_context.OsteologySkulls
                .Where(m => m.Id == id || id == null)
                .OrderBy(m => m.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList()),

                PageNumberingInfo = new PageNumberingInfo
                {
                    NumItemsPerPage = pageSize,
                    CurrentPage = pageNum,

                    //if no team has been selected, then get the full count. Otherwise only count the number from the team name that has been selected
                    TotalNumItems = (id == null ? _context.OsteologySkulls.Count() :
                     _context.OsteologySkulls.Where(x => x.Id == id).Count())
                },


            });
        }

        public IActionResult DemographicPublic(string? id, int pageNum = 1)
        {
            //return View(context.Recipes
            //    .FromSqlInterpolated($"SELECT * FROM Recipes WHERE RecipeClassId = {mealtypeid} OR {mealtypeid} IS NULL")
            //    .ToList());

            int pageSize = 50;

            return View(new IndexViewModel
            {
                Demographics = (_context.Demographics
                .Where(m => m.Id == id || id == null)
                .OrderBy(m => m.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList()),

                PageNumberingInfo = new PageNumberingInfo
                {
                    NumItemsPerPage = pageSize,
                    CurrentPage = pageNum,

                    //if no team has been selected, then get the full count. Otherwise only count the number from the team name that has been selected
                    TotalNumItems = (id == null ? _context.Demographics.Count() :
                     _context.Demographics.Where(x => x.Id == id).Count())
                },


            });
        }

        public IActionResult ArtifactBioNotePublic(string? id, int pageNum = 1)
        {
            //return View(context.Recipes
            //    .FromSqlInterpolated($"SELECT * FROM Recipes WHERE RecipeClassId = {mealtypeid} OR {mealtypeid} IS NULL")
            //    .ToList());

            int pageSize = 50;

            return View(new IndexViewModel
            {
                ArtifactBioNotes = (_context.ArtifactBioNotes
                .Where(m => m.Id == id || id == null)
                .OrderBy(m => m.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList()),

                PageNumberingInfo = new PageNumberingInfo
                {
                    NumItemsPerPage = pageSize,
                    CurrentPage = pageNum,

                    //if no team has been selected, then get the full count. Otherwise only count the number from the team name that has been selected
                    TotalNumItems = (id == null ? _context.ArtifactBioNotes.Count() :
                     _context.ArtifactBioNotes.Where(x => x.Id == id).Count())
                },


            });
        }
        #endregion

        #region R View

        [Authorize]
        public IActionResult ViewBurialsResearchers(string? id, int pageNum = 1)
        {
            int pageSize = 50;

            return View(new IndexViewModel
            {
                LocationMeasurementsR = (_context.LocationMeasurements
                .Where(m => m.Id == id || id == null)
                .OrderBy(m => m.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList()),

                PageNumberingInfo = new PageNumberingInfo
                {
                    NumItemsPerPage = pageSize,
                    CurrentPage = pageNum,

                    //if no team has been selected, then get the full count. Otherwise only count the number from the team name that has been selected
                    TotalNumItems = (id == null ? _context.LocationMeasurements.Count() :
                     _context.LocationMeasurements.Where(x => x.Id == id).Count())
                },

            });
        }

        public IActionResult OsteologyR(string? id, int pageNum = 1)
        {
            int pageSize = 50;

            return View(new IndexViewModel
            {
                OsteologiesR = (_context.Osteologies
                .Where(m => m.Id == id || id == null)
                .OrderBy(m => m.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList()),

                PageNumberingInfo = new PageNumberingInfo
                {
                    NumItemsPerPage = pageSize,
                    CurrentPage = pageNum,

                    //if no team has been selected, then get the full count. Otherwise only count the number from the team name that has been selected
                    TotalNumItems = (id == null ? _context.Osteologies.Count() :
                     _context.Osteologies.Where(x => x.Id == id).Count())
                },

            });
        }

        public IActionResult OsteologySkullR(string? id, int pageNum = 1)
        {
            int pageSize = 50;

            return View(new IndexViewModel
            {
                OsteologySkullsR = (_context.OsteologySkulls
                .Where(m => m.Id == id || id == null)
                .OrderBy(m => m.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList()),

                PageNumberingInfo = new PageNumberingInfo
                {
                    NumItemsPerPage = pageSize,
                    CurrentPage = pageNum,

                    //if no team has been selected, then get the full count. Otherwise only count the number from the team name that has been selected
                    TotalNumItems = (id == null ? _context.OsteologySkulls.Count() :
                     _context.OsteologySkulls.Where(x => x.Id == id).Count())
                },

            });
        }

        public IActionResult DemographicR(string? id, int pageNum = 1)
        {
            int pageSize = 50;

            return View(new IndexViewModel
            {
                DemographicsR = (_context.Demographics
                .Where(m => m.Id == id || id == null)
                .OrderBy(m => m.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList()),

                PageNumberingInfo = new PageNumberingInfo
                {
                    NumItemsPerPage = pageSize,
                    CurrentPage = pageNum,

                    //if no team has been selected, then get the full count. Otherwise only count the number from the team name that has been selected
                    TotalNumItems = (id == null ? _context.Demographics.Count() :
                     _context.Demographics.Where(x => x.Id == id).Count())
                },

            });
        }

        public IActionResult ArtifactBioNoteR(string? id, int pageNum = 1)
        {
            int pageSize = 50;

            return View(new IndexViewModel
            {
                ArtifactBioNotesR = (_context.ArtifactBioNotes
                .Where(m => m.Id == id || id == null)
                .OrderBy(m => m.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList()),

                PageNumberingInfo = new PageNumberingInfo
                {
                    NumItemsPerPage = pageSize,
                    CurrentPage = pageNum,

                    //if no team has been selected, then get the full count. Otherwise only count the number from the team name that has been selected
                    TotalNumItems = (id == null ? _context.ArtifactBioNotes.Count() :
                     _context.ArtifactBioNotes.Where(x => x.Id == id).Count())
                },

            });
        }
        #endregion

        #region CRUD FOR DATA AND BIO NOTES
        [HttpGet]
        public IActionResult CRUD_AddData()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CRUD_AddData(LocationMeasurement newdata)
        {
            if (ModelState.IsValid)
            {
                _context.LocationMeasurements.Add(newdata);
                _context.SaveChanges();

                return RedirectToAction("ViewBurialsResearchers");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult CRUD_UpdateData(string id)
        {
            LocationMeasurement data = _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault();
            return View(data);
        }

        [HttpPost]
        public IActionResult CRUD_UpdateData(LocationMeasurement data, string id)
        {
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().Nors = data.Nors;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().HighNs = data.HighNs;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().LowNs = data.LowNs;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().Eorw = data.Eorw;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().HighEw = data.HighEw;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().LowEw = data.LowEw;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().Square = data.Square;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().BurialNum = data.BurialNum;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().Direction = data.Direction;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().Depth = data.Depth;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().HeadSouth = data.HeadSouth;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().FeetSouth = data.FeetSouth;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().HeadWest = data.HeadWest;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().FeetWest = data.FeetWest;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().BurialLength = data.BurialLength;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().DiscoveryDay = data.DiscoveryDay;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().DiscoveryMonth = data.DiscoveryMonth;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().DiscoveryYear = data.DiscoveryYear;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().Cluster = data.Cluster;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().BurialRack = data.BurialRack;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().CreatedBy = data.CreatedBy;
            _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault().LastModifiedBy = data.LastModifiedBy;

            _context.SaveChanges();

            return RedirectToAction("ViewBurialsResearchers");
        }

        [HttpPost]
        public IActionResult CRUD_DeleteData(string id)
        {
            var data = _context.LocationMeasurements.Where(m => m.Id == id).FirstOrDefault();
            _context.LocationMeasurements.Remove(data);
            _context.SaveChangesAsync();

            return RedirectToAction("ViewBurialsResearchers");
        }

        [HttpGet]
        public IActionResult CRUD_AddNote()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CRUD_AddNote(ArtifactBioNote newdata)
        {
            if (ModelState.IsValid)
            {
                _context.ArtifactBioNotes.Add(newdata);
                _context.SaveChanges();

                return RedirectToAction("ArtifactBioNoteR");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult CRUD_UpdateNote(string id)
        {
            ArtifactBioNote data = _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault();
            return View(data);
        }

        [HttpPost]
        public IActionResult CRUD_UpdateNote(ArtifactBioNote data, string id)
        {
            _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault().Rack = data.Rack;
            _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault().ArtifactFound = data.ArtifactFound;
            _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault().ArtifactDescription = data.ArtifactDescription;
            _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault().SampleTaken = data.SampleTaken;
            _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault().BioNotes = data.BioNotes;
            _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault().AdditionalNotes = data.AdditionalNotes;
            _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault().FaceBundle = data.FaceBundle;
            _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault().PathologyAnomalies = data.PathologyAnomalies;
            _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault().BurialWraping = data.BurialWraping;
            _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault().PreservationIndex = data.PreservationIndex;
            _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault().CreatedBy = data.CreatedBy;
            _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault().LastModifiedBy = data.LastModifiedBy;

            _context.SaveChanges();

            return RedirectToAction("ArtifactBioNoteR");
        }

        [HttpPost]
        public IActionResult CRUD_DeleteNote(string id)
        {
            var data = _context.ArtifactBioNotes.Where(m => m.Id == id).FirstOrDefault();
            _context.ArtifactBioNotes.Remove(data);
            _context.SaveChangesAsync();

            return RedirectToAction("ArtifactBioNoteR");
        }

        #endregion

        [Authorize]
        public IActionResult ResearchersTools()
        {
            //return View();
            return View();
        }
        //[Authorize]
        //public IActionResult EnterFieldNotes()
        //{
        //    //return View();
        //    return View();
        //}

        [Authorize]
        public IActionResult EnterFieldNotes(string Id)
        {

            return View();
        }


        [Authorize]
        [HttpGet]
        public IActionResult Demographic(string Id)
        {
            if (Id != null)
            {
                return View(new DemographicViewModel
                {
                    Demographic = _context.Demographics.FirstOrDefault(o => o.Id == Id),
                    Identifier = Id
                });
            }
            else
            {
                return View();
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Demographic(DemographicViewModel viewModel)
        {
            if (viewModel.Identifier != null)
            {
                //we either have demographic already, or the ID that needs demographic information was sent
                if(_context.Demographics.FirstOrDefault(o => o.Id == viewModel.Identifier) != null){
                    //we have an existing Demographic'
                    _context.Demographics.FirstOrDefault(o => o.Id == viewModel.Identifier).AgeAtDeath = viewModel.Demographic.AgeAtDeath;
                    _context.Demographics.FirstOrDefault(o => o.Id == viewModel.Identifier).AgeAtDeath = viewModel.Demographic.AgeAtDeath;
                    _context.Demographics.FirstOrDefault(o => o.Id == viewModel.Identifier).AgeAtDeath = viewModel.Demographic.AgeAtDeath;
                    _context.Demographics.FirstOrDefault(o => o.Id == viewModel.Identifier).AgeAtDeath = viewModel.Demographic.AgeAtDeath;
                    _context.Demographics.FirstOrDefault(o => o.Id == viewModel.Identifier).AgeAtDeath = viewModel.Demographic.AgeAtDeath;
                    _context.Demographics.FirstOrDefault(o => o.Id == viewModel.Identifier).AgeAtDeath = viewModel.Demographic.AgeAtDeath;
                    _context.Demographics.FirstOrDefault(o => o.Id == viewModel.Identifier).AgeAtDeath = viewModel.Demographic.AgeAtDeath;
                    _context.Demographics.FirstOrDefault(o => o.Id == viewModel.Identifier).AgeAtDeath = viewModel.Demographic.AgeAtDeath;

                    _context.Demographics.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = viewModel.Demographic.LastModifiedBy;
                    _context.Demographics.FirstOrDefault(o => o.Id == viewModel.Identifier).AgeAtDeath = viewModel.Demographic.AgeAtDeath;



                    _context.SaveChanges();
                    return View();
                }
                else
                {
                    //we are adding a new one, with a shared id
                    viewModel.Demographic.Id = viewModel.Identifier;

                    _context.Demographics.Add(viewModel.Demographic);
                    _context.SaveChanges();


                    //return next view
                    return View("EditOsteology", viewModel.Identifier);
                }
            }
            else
            {
                //create new
                //creates without connection to location measurement (not recommended)
                //they are entering a new mummy
                _context.Demographics.Add(viewModel.Demographic);
                _context.SaveChanges();
                //set timestamp

                _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedTimestamp = DateTime.Now; 
                _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = User.FindFirst(ClaimTypes.Email).Value;
                _context.SaveChanges();

                //either take them to location measurement or we shouldnt allow this
                return View();
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult LocationMeasurement(string Id)
        {
            if (Id != null)
            {
                return View(new LocationMeasurementViewModel
                {
                    LocationMeasurement = _context.LocationMeasurements.FirstOrDefault(o => o.Id == Id),
                    Identifier = Id
                });
            }
            else
            {
                return View(new LocationMeasurementViewModel());
            }
        }
        [Authorize]
        [HttpPost]
        public IActionResult LocationMeasurement(LocationMeasurementViewModel viewModel)
        {
            if(viewModel.Identifier != null)
            {
                var recalculatedIdentifier = viewModel.LocationMeasurement.Nors + viewModel.LocationMeasurement.LowNs.ToString() + viewModel.LocationMeasurement.Eorw + viewModel.LocationMeasurement.LowEw.ToString() + viewModel.LocationMeasurement.Square + viewModel.LocationMeasurement.BurialNum.ToString();
                //they are editing an existing mummy
                _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).Nors = viewModel.LocationMeasurement.Nors;
                _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).LowNs = viewModel.LocationMeasurement.LowNs;
                _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).HighNs = (Int32.Parse(viewModel.LocationMeasurement.LowNs) + 10).ToString();
                _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).LowEw = viewModel.LocationMeasurement.LowEw;
                _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).HighEw = (Int32.Parse(viewModel.LocationMeasurement.LowEw) + 10).ToString();
                _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).Square = viewModel.LocationMeasurement.Square;
                _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).BurialNum = viewModel.LocationMeasurement.BurialNum;

                _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).Direction = viewModel.LocationMeasurement.Direction;
                _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).Depth = viewModel.LocationMeasurement.Depth;
                _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).HeadSouth = viewModel.LocationMeasurement.HeadSouth;
                _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).FeetSouth = viewModel.LocationMeasurement.FeetSouth;
                _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).HeadWest = viewModel.LocationMeasurement.HeadWest;
                _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).FeetWest = viewModel.LocationMeasurement.FeetWest;
                _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).BurialLength = viewModel.LocationMeasurement.BurialLength;
                _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).DiscoveryDay = viewModel.LocationMeasurement.DiscoveryDay;
                _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).DiscoveryMonth = viewModel.LocationMeasurement.DiscoveryMonth;
                _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).DiscoveryYear = viewModel.LocationMeasurement.DiscoveryYear;
                _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).Cluster = viewModel.LocationMeasurement.Cluster;
                _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).BurialRack = viewModel.LocationMeasurement.BurialRack;
                _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = viewModel.LocationMeasurement.LastModifiedBy;
                _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedTimestamp = DateTime.Now; 
                _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = User.FindFirst(ClaimTypes.Email).Value;


                _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).Id = recalculatedIdentifier;
                _context.SaveChanges();
            }
            else
            {
                var calculatedIdentifier = viewModel.LocationMeasurement.Nors + viewModel.LocationMeasurement.LowNs.ToString() + viewModel.LocationMeasurement.Eorw + viewModel.LocationMeasurement.LowEw.ToString() + viewModel.LocationMeasurement.Square + viewModel.LocationMeasurement.BurialNum.ToString();
                viewModel.LocationMeasurement.Id = calculatedIdentifier;
                //they are entering a new mummy
                _context.LocationMeasurements.Add(viewModel.LocationMeasurement);
                _context.SaveChanges();
                //set timestamp

                _context.LocationMeasurements.FirstOrDefault(o => o.Id == calculatedIdentifier).LastModifiedTimestamp = DateTime.Now; 
                _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = User.FindFirst(ClaimTypes.Email).Value;
                _context.SaveChanges();

                //route to the osteology entry view
                return View("Demographic", viewModel.Identifier);
            }
            return RedirectToAction("Index");
        }

        //osteology edit
        [Authorize]
        [HttpGet]
        public IActionResult EditOsteology(string Id)
        {
            if (Id != null)
            {
                return View(new OsteologyViewModel
                {
                    Osteology = _context.Osteologies.FirstOrDefault(o => o.Id == Id),
                    Identifier = Id
                });
            }
            else
            {
                return View(new OsteologyViewModel());
            }
        }
        [Authorize]
        [HttpPost]
        public IActionResult EditOsteology(OsteologyViewModel viewModel)
        {
            if (viewModel.Identifier != null)
            {
                if (_context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier) != null)
                {
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).BasilarSuture = viewModel.Osteology.BasilarSuture;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).VentralArc = viewModel.Osteology.VentralArc;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).SubpubicAngle = viewModel.Osteology.SubpubicAngle;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).SciaticNotch = viewModel.Osteology.SciaticNotch;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).PubicBone = viewModel.Osteology.PubicBone;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).PreaurSulcus = viewModel.Osteology.PreaurSulcus;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).MedialIpramus = viewModel.Osteology.MedialIpramus;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).DorsalPitting = viewModel.Osteology.DorsalPitting;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).ForemanMagnum = viewModel.Osteology.ForemanMagnum;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).FemurHead = viewModel.Osteology.FemurHead;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).HumerusHead = viewModel.Osteology.HumerusHead;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).Osteophytosis = viewModel.Osteology.Osteophytosis;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).PubicSymphysis = viewModel.Osteology.PubicSymphysis;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).BoneLength = viewModel.Osteology.BoneLength;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).MedialClavicle = viewModel.Osteology.MedialClavicle;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).IliacCrest = viewModel.Osteology.IliacCrest;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).FemurDiameter = viewModel.Osteology.FemurDiameter;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).Humerus = viewModel.Osteology.Humerus;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).FemurLength = viewModel.Osteology.FemurLength;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).HumerusLength = viewModel.Osteology.HumerusLength;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).TibiaLength = viewModel.Osteology.TibiaLength;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).PostcraniaTrauma = viewModel.Osteology.PostcraniaTrauma;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).ToothAttrition = viewModel.Osteology.ToothAttrition;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).ToothEruption = viewModel.Osteology.ToothEruption;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).CreatedBy = viewModel.Osteology.CreatedBy;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = viewModel.Osteology.LastModifiedBy;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedTimestamp = DateTime.Now; 
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = User.FindFirst(ClaimTypes.Email).Value;

                    _context.SaveChanges();
                    return View();
                }
            }
            else
            {
                _context.Osteologies.Add(viewModel.Osteology);
                _context.SaveChanges();

                _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedTimestamp = DateTime.Now; 
                _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = User.FindFirst(ClaimTypes.Email).Value;
                _context.SaveChanges();
                return View("EditSkull", viewModel.Identifier);
            }
            return RedirectToAction("Index");
        }

        //edit skull actions
        [Authorize]
        [HttpGet]
        public IActionResult EditSkull(string Id)
        {
            if (Id != null)
            {
                return View(new SkullViewModel
                {
                    OsteologySkull = _context.OsteologySkulls.FirstOrDefault(o => o.Id == Id),
                    Identifier = Id
                });
            }
            else
            {
                return View(new SkullViewModel());
            }
        }
        [Authorize]
        [HttpPost]
        public IActionResult EditSkull(SkullViewModel viewModel)
        {
            if (viewModel.Identifier != null)
            {
                if (_context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier) != null)
                {
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).MaxCranialLength = viewModel.OsteologySkull.MaxCranialLength;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).MaxCranialBreadth = viewModel.OsteologySkull.MaxCranialBreadth;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).BasionBregmaHeight = viewModel.OsteologySkull.BasionBregmaHeight;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).BasionNasion = viewModel.OsteologySkull.BasionNasion;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).BasionProsthionLength = viewModel.OsteologySkull.BasionProsthionLength;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).NasionProsthion = viewModel.OsteologySkull.NasionProsthion;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).MaxNasalBreadth = viewModel.OsteologySkull.MaxNasalBreadth;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).InterorbitalBreadth = viewModel.OsteologySkull.InterorbitalBreadth;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).BizygomaticDiameter = viewModel.OsteologySkull.BizygomaticDiameter;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).CranialSuture = viewModel.OsteologySkull.CranialSuture;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).ZygomaticCrest = viewModel.OsteologySkull.ZygomaticCrest;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).NuchalCrest = viewModel.OsteologySkull.NuchalCrest;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).Gonian = viewModel.OsteologySkull.Gonian;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).ParietalBossing = viewModel.OsteologySkull.ParietalBossing;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).OrbitEdge = viewModel.OsteologySkull.OrbitEdge;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).SupraorbitalRidges = viewModel.OsteologySkull.SupraorbitalRidges;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).Robust = viewModel.OsteologySkull.Robust;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).SkullTrauma = viewModel.OsteologySkull.SkullTrauma;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).CreatedBy = viewModel.OsteologySkull.CreatedBy;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = viewModel.OsteologySkull.LastModifiedBy;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedTimestamp = DateTime.Now; 
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = User.FindFirst(ClaimTypes.Email).Value;

                    _context.SaveChanges();

                    return View();
                }
            }
            else
            {
                _context.OsteologySkulls.Add(viewModel.OsteologySkull);
                _context.SaveChanges();

                _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedTimestamp = DateTime.Now; 
                _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = User.FindFirst(ClaimTypes.Email).Value;
                _context.SaveChanges();

                return View("EditArtifactBio", viewModel.Identifier);
            }
            return RedirectToAction("Index");
        }

        //edit bio notes
        [Authorize]
        [HttpGet]
        public IActionResult EditArtifactBio(string Id)
        {
            if (Id != null)
            {
                return View(new ArtifactViewModel
                {
                    ArtifactBioNote = _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == Id),
                    Identifier = Id
                });
            }
            else
            {
                return View(new ArtifactViewModel());
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditArtifactBio(ArtifactViewModel viewModel)
        {
            if (viewModel.Identifier != null)
            {
                if (_context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier) != null)
                {
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).Rack = viewModel.ArtifactBioNote.Rack;
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).ArtifactFound = viewModel.ArtifactBioNote.ArtifactFound;
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).ArtifactDescription = viewModel.ArtifactBioNote.ArtifactDescription;
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).SampleTaken = viewModel.ArtifactBioNote.SampleTaken;
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).BioNotes = viewModel.ArtifactBioNote.BioNotes;
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).AdditionalNotes = viewModel.ArtifactBioNote.AdditionalNotes;
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).FaceBundle = viewModel.ArtifactBioNote.FaceBundle;
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).PathologyAnomalies = viewModel.ArtifactBioNote.PathologyAnomalies;
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).BurialWraping = viewModel.ArtifactBioNote.BurialWraping;
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).PreservationIndex = viewModel.ArtifactBioNote.PreservationIndex;
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).CreatedBy = viewModel.ArtifactBioNote.CreatedBy;
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = User.FindFirst(ClaimTypes.Email).Value;
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedTimestamp = DateTime.Now; 
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = User.FindFirst(ClaimTypes.Email).Value;

                    _context.SaveChanges();

                    return View();
                }
            }
            else
            {
                _context.ArtifactBioNotes.Add(viewModel.ArtifactBioNote);
                _context.SaveChanges();

                _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedTimestamp = DateTime.Now; 
                _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = User.FindFirst(ClaimTypes.Email).Value;
                

                _context.SaveChanges();

                return View("ViewBurialPublic");
            }
            return RedirectToAction("Index");
        }

        //EDIT DATA INFORMATION
        [Authorize]
        [HttpGet]
        public IActionResult Edit(string Id)
        {
            if (Id != null)
            {
                return View(new FieldNotesViewModel
                {
                    Osteology = _context.Osteologies.FirstOrDefault(o => o.Id == Id),
                    OsteologySkull = _context.OsteologySkulls.FirstOrDefault(o => o.Id == Id),
                    LocationMeasurement = _context.LocationMeasurements.FirstOrDefault(o => o.Id == Id),
                    Demographic = _context.Demographics.FirstOrDefault(o => o.Id == Id),
                    ArtifactBioNote = _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == Id),
                    Identifier = Id
                });
            }
            else
            {
                return View((new FieldNotesViewModel()));
            }
        }
        [Authorize]
        [HttpPost]
        public IActionResult Edit(FieldNotesViewModel viewModel)
        {
            if (viewModel.Identifier != null)
            {
                if (_context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier) != null)
                {
                    //LOCATION
                    var recalculatedIdentifier = viewModel.LocationMeasurement.Nors + viewModel.LocationMeasurement.LowNs.ToString() + viewModel.LocationMeasurement.Eorw + viewModel.LocationMeasurement.LowEw.ToString() + viewModel.LocationMeasurement.Square + viewModel.LocationMeasurement.BurialNum.ToString();
                    //they are editing an existing mummy
                    _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).Nors = viewModel.LocationMeasurement.Nors;
                    _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).LowNs = viewModel.LocationMeasurement.LowNs;
                    _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).HighNs = (Int32.Parse(viewModel.LocationMeasurement.LowNs) + 10).ToString();
                    _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).LowEw = viewModel.LocationMeasurement.LowEw;
                    _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).HighEw = (Int32.Parse(viewModel.LocationMeasurement.LowEw) + 10).ToString();
                    _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).Square = viewModel.LocationMeasurement.Square;
                    _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).BurialNum = viewModel.LocationMeasurement.BurialNum;

                    _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).Direction = viewModel.LocationMeasurement.Direction;
                    _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).Depth = viewModel.LocationMeasurement.Depth;
                    _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).HeadSouth = viewModel.LocationMeasurement.HeadSouth;
                    _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).FeetSouth = viewModel.LocationMeasurement.FeetSouth;
                    _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).HeadWest = viewModel.LocationMeasurement.HeadWest;
                    _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).FeetWest = viewModel.LocationMeasurement.FeetWest;
                    _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).BurialLength = viewModel.LocationMeasurement.BurialLength;
                    _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).DiscoveryDay = viewModel.LocationMeasurement.DiscoveryDay;
                    _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).DiscoveryMonth = viewModel.LocationMeasurement.DiscoveryMonth;
                    _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).DiscoveryYear = viewModel.LocationMeasurement.DiscoveryYear;
                    _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).Cluster = viewModel.LocationMeasurement.Cluster;
                    _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).BurialRack = viewModel.LocationMeasurement.BurialRack;
                    _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = viewModel.LocationMeasurement.LastModifiedBy;
                    _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedTimestamp = DateTime.Now; 
                    _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = User.FindFirst(ClaimTypes.Email).Value;
                    _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).Id = recalculatedIdentifier;
                    _context.SaveChanges();
                }
                else
                {
                    var calculatedIdentifier = viewModel.LocationMeasurement.Nors + viewModel.LocationMeasurement.LowNs.ToString() + viewModel.LocationMeasurement.Eorw + viewModel.LocationMeasurement.LowEw.ToString() + viewModel.LocationMeasurement.Square + viewModel.LocationMeasurement.BurialNum.ToString();
                    viewModel.LocationMeasurement.Id = calculatedIdentifier;
                    //they are entering a new mummy
                    _context.LocationMeasurements.Add(viewModel.LocationMeasurement);
                    _context.SaveChanges();
                    //set timestamp
                    _context.LocationMeasurements.FirstOrDefault(o => o.Id == calculatedIdentifier).LastModifiedTimestamp = DateTime.Now; 
                    _context.LocationMeasurements.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = User.FindFirst(ClaimTypes.Email).Value;
                    _context.SaveChanges();
                }

                if (_context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier) != null)
                {
                    //OSTEOLOGY
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).BasilarSuture = viewModel.Osteology.BasilarSuture;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).VentralArc = viewModel.Osteology.VentralArc;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).SubpubicAngle = viewModel.Osteology.SubpubicAngle;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).SciaticNotch = viewModel.Osteology.SciaticNotch;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).PubicBone = viewModel.Osteology.PubicBone;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).PreaurSulcus = viewModel.Osteology.PreaurSulcus;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).MedialIpramus = viewModel.Osteology.MedialIpramus;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).DorsalPitting = viewModel.Osteology.DorsalPitting;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).ForemanMagnum = viewModel.Osteology.ForemanMagnum;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).FemurHead = viewModel.Osteology.FemurHead;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).HumerusHead = viewModel.Osteology.HumerusHead;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).Osteophytosis = viewModel.Osteology.Osteophytosis;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).PubicSymphysis = viewModel.Osteology.PubicSymphysis;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).BoneLength = viewModel.Osteology.BoneLength;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).MedialClavicle = viewModel.Osteology.MedialClavicle;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).IliacCrest = viewModel.Osteology.IliacCrest;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).FemurDiameter = viewModel.Osteology.FemurDiameter;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).Humerus = viewModel.Osteology.Humerus;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).FemurLength = viewModel.Osteology.FemurLength;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).HumerusLength = viewModel.Osteology.HumerusLength;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).TibiaLength = viewModel.Osteology.TibiaLength;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).PostcraniaTrauma = viewModel.Osteology.PostcraniaTrauma;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).ToothAttrition = viewModel.Osteology.ToothAttrition;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).ToothEruption = viewModel.Osteology.ToothEruption;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).CreatedBy = viewModel.Osteology.CreatedBy;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = viewModel.Osteology.LastModifiedBy;
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedTimestamp = DateTime.Now; 
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = User.FindFirst(ClaimTypes.Email).Value;
                    _context.SaveChanges();
                }
                else
                {
                    _context.Osteologies.Add(viewModel.Osteology);
                    _context.SaveChanges();
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedTimestamp = DateTime.Now; 
                    _context.Osteologies.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = User.FindFirst(ClaimTypes.Email).Value;
                    _context.SaveChanges();
                }

                if (_context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier) != null)
                {
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).MaxCranialLength = viewModel.OsteologySkull.MaxCranialLength;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).MaxCranialBreadth = viewModel.OsteologySkull.MaxCranialBreadth;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).BasionBregmaHeight = viewModel.OsteologySkull.BasionBregmaHeight;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).BasionNasion = viewModel.OsteologySkull.BasionNasion;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).BasionProsthionLength = viewModel.OsteologySkull.BasionProsthionLength;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).NasionProsthion = viewModel.OsteologySkull.NasionProsthion;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).MaxNasalBreadth = viewModel.OsteologySkull.MaxNasalBreadth;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).InterorbitalBreadth = viewModel.OsteologySkull.InterorbitalBreadth;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).BizygomaticDiameter = viewModel.OsteologySkull.BizygomaticDiameter;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).CranialSuture = viewModel.OsteologySkull.CranialSuture;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).ZygomaticCrest = viewModel.OsteologySkull.ZygomaticCrest;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).NuchalCrest = viewModel.OsteologySkull.NuchalCrest;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).Gonian = viewModel.OsteologySkull.Gonian;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).ParietalBossing = viewModel.OsteologySkull.ParietalBossing;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).OrbitEdge = viewModel.OsteologySkull.OrbitEdge;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).SupraorbitalRidges = viewModel.OsteologySkull.SupraorbitalRidges;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).Robust = viewModel.OsteologySkull.Robust;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).SkullTrauma = viewModel.OsteologySkull.SkullTrauma;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).CreatedBy = viewModel.OsteologySkull.CreatedBy;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = viewModel.OsteologySkull.LastModifiedBy;
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedTimestamp = DateTime.Now; 
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = User.FindFirst(ClaimTypes.Email).Value;
                    _context.SaveChanges();
                }
                else
                {
                    _context.OsteologySkulls.Add(viewModel.OsteologySkull);
                    _context.SaveChanges();
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedTimestamp = DateTime.Now; 
                    _context.OsteologySkulls.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = User.FindFirst(ClaimTypes.Email).Value;
                    _context.SaveChanges();
                }

                if (_context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier) != null)
                {
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).Rack = viewModel.ArtifactBioNote.Rack;
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).ArtifactFound = viewModel.ArtifactBioNote.ArtifactFound;
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).ArtifactDescription = viewModel.ArtifactBioNote.ArtifactDescription;
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).SampleTaken = viewModel.ArtifactBioNote.SampleTaken;
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).BioNotes = viewModel.ArtifactBioNote.BioNotes;
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).AdditionalNotes = viewModel.ArtifactBioNote.AdditionalNotes;
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).FaceBundle = viewModel.ArtifactBioNote.FaceBundle;
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).PathologyAnomalies = viewModel.ArtifactBioNote.PathologyAnomalies;
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).BurialWraping = viewModel.ArtifactBioNote.BurialWraping;
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).PreservationIndex = viewModel.ArtifactBioNote.PreservationIndex;
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).CreatedBy = viewModel.ArtifactBioNote.CreatedBy;
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = viewModel.ArtifactBioNote.LastModifiedBy;
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedTimestamp = DateTime.Now;
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = User.FindFirst(ClaimTypes.Email).Value;
                    _context.SaveChanges();
                }
                else
                {
                    _context.ArtifactBioNotes.Add(viewModel.ArtifactBioNote);
                    _context.SaveChanges();
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedTimestamp = DateTime.Now; 
                    _context.ArtifactBioNotes.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = User.FindFirst(ClaimTypes.Email).Value;
                    _context.SaveChanges();
                }

                if (_context.Demographics.FirstOrDefault(o => o.Id == viewModel.Identifier) != null)
                {
                    //we have an existing Demographic'
                    _context.Demographics.FirstOrDefault(o => o.Id == viewModel.Identifier).AgeAtDeath = viewModel.Demographic.AgeAtDeath;
                    _context.Demographics.FirstOrDefault(o => o.Id == viewModel.Identifier).AgeAtDeath = viewModel.Demographic.AgeAtDeath;
                    _context.Demographics.FirstOrDefault(o => o.Id == viewModel.Identifier).AgeAtDeath = viewModel.Demographic.AgeAtDeath;
                    _context.Demographics.FirstOrDefault(o => o.Id == viewModel.Identifier).AgeAtDeath = viewModel.Demographic.AgeAtDeath;
                    _context.Demographics.FirstOrDefault(o => o.Id == viewModel.Identifier).AgeAtDeath = viewModel.Demographic.AgeAtDeath;
                    _context.Demographics.FirstOrDefault(o => o.Id == viewModel.Identifier).AgeAtDeath = viewModel.Demographic.AgeAtDeath;
                    _context.Demographics.FirstOrDefault(o => o.Id == viewModel.Identifier).AgeAtDeath = viewModel.Demographic.AgeAtDeath;
                    _context.Demographics.FirstOrDefault(o => o.Id == viewModel.Identifier).AgeAtDeath = viewModel.Demographic.AgeAtDeath;

                    _context.Demographics.FirstOrDefault(o => o.Id == viewModel.Identifier).LastModifiedBy = viewModel.Demographic.LastModifiedBy;
                    _context.Demographics.FirstOrDefault(o => o.Id == viewModel.Identifier).AgeAtDeath = viewModel.Demographic.AgeAtDeath;

                    _context.SaveChanges();
                }
                else
                {
                    //we are adding a new one, with a shared id
                    viewModel.Demographic.Id = viewModel.Identifier;
                    _context.Demographics.Add(viewModel.Demographic);
                    _context.SaveChanges();
                }
            }
            else
            {
                _context.LocationMeasurements.Add(viewModel.LocationMeasurement);
                _context.Osteologies.Add(viewModel.Osteology);
                _context.OsteologySkulls.Add(viewModel.OsteologySkull);
                _context.LocationMeasurements.Add(viewModel.LocationMeasurement);
                _context.Demographics.Add(viewModel.Demographic);
            }

            return View();
        }


            [Authorize]
        public IActionResult ViewFieldNotes()
        {
            //return View();
            return View();
        }
       
        [HttpGet]
        [Authorize]
        public IActionResult TestForm()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> TestForm(string test, string username)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(username);
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
