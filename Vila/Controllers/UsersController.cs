using AutoMapper;
using Castle.Core.Internal;
using Castle.Core.Resource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Vila.Data;
using Vila.Data.DTO;
using Vila.Data.Entities;
using Vila.Helpers;
using Vila.Services;
using static System.Net.Mime.MediaTypeNames;

namespace Vila.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly VilaDbContext _db;
        private readonly IMapper _mapper;

        public UsersController(VilaDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
           

        }

        //[Authorize(Roles = "Admin")] אפשר להוסיף בשביל תנאי
        [HttpGet]
        [Route("")]
        public IActionResult getAllUser()
        {
            List<UsersEO> users = _db.Users.ToList();
            return Ok(users);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UsersEO userObj)
        {
            if (userObj == null)
                return BadRequest();

            var user = await _db.Users
                .FirstOrDefaultAsync(x => x.Username == userObj.Username);
            if (user == null)
                return NotFound(new { Message = "!משתמש לא נמצא" });

            if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password))
                return BadRequest(new { Message = "סיסמה לא נכונה" });

            user.Token = CreateJwt(user); // גרסה של ההודי

            return Ok(new
            {

                Token = user.Token,
                Message = "התחברת בהצלחה"
            });
        }

        [HttpPost]
        [Route("rregister")]
        public IActionResult Addrregister([FromBody] UsersEO user)
        {
            if (user == null)
                return BadRequest();
            UsersEO ValidEmail = _db.Users.FirstOrDefault(o => o.Email == user.Email);
            UsersEO ValidUserName = _db.Users.FirstOrDefault(o => o.Username == user.Username);

            // Check email
            if (ValidEmail != null)
            {
                return NotFound(new { Message = "כבר קיים משתמש מקושר למייל הזה" });
            }

            // Check User name
            if (ValidUserName != null)
            {
                return NotFound(new { Message = "שם משתמש תפוס, נא לבחור שם משתמש אחר" });
            }

            // Check password Strength
            var pass = CheckPasswordStrength(user.Password);
            if (!string.IsNullOrEmpty(pass))
                return BadRequest(new { Message = pass.ToString() });


            user.Password = PasswordHasher.HashPassword(user.Password);
            user.Role = "User";
            user.Token = "";
            _db.Users.Add(user);
            _db.SaveChanges();
            return Ok(new
            {
                Message = "נרשמת בהצלחה"
            });
        }


        // test areia
        [HttpGet]
        [Route("test")]
        public IActionResult getListTest()
        {
            List<Test> _t = _db.t.ToList();
            return Ok(_t);
        }

        [HttpPost]
        [Route("AddTest")]
        public IActionResult AddName([FromBody] Test _Test)
        {
            Test ValidName = _db.t.FirstOrDefault(n => n.name == _Test.name);
            if (ValidName != null)
            {
                return BadRequest("כבר קיים שם כזה");
            }
            _db.t.Add(_Test);
            _db.SaveChanges();
            return Ok(new
            {
                Message = " טסט נוסף בהצלחה"
            });
        }




        // check password
        private string CheckPasswordStrength(string password)
        {
            StringBuilder sb = new StringBuilder();
            if (password.Length < 8)
                sb.Append("הסיסמה צריכה להכיל לפחות 8 תווים" + Environment.NewLine);
            if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]")
                && Regex.IsMatch(password, "[0-9]")))
                sb.Append("הסיסמה צריכה להכיל אותיות קטנות, גדולות, ומספרים" + Environment.NewLine);
            if (!(Regex.IsMatch(password, "[@,!,#,$,*,&]")))
                sb.Append("@,!,#,$,*,& הסיסמה צריכה להכיל לפחות אחד מהתווים הבאים " + Environment.NewLine);
            return sb.ToString();
        }


        // Create Token
        // גירסה של ההודי

        private string CreateJwt(UsersEO user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryveryscret.....");
            var identity = new ClaimsIdentity(new Claim[]
            {// אפשר להוסיף עוד שדות שיופיעו בטוקן
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name,$"{user.FirstName} "),
                new Claim(ClaimTypes.Email,$"{user.Email}  "),
                new Claim(ClaimTypes.Surname,$" {user.LastName} "),
                new Claim(ClaimTypes.SerialNumber,$"{user.UserID}"),
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1), // הטוקן תקף ליום 
                                                   //  Expires = DateTime.Now.AddSeconds(10), // הטוקן תקף ל 10 שניות
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }




        /*
        // Reset Password e-mail
        [HttpPost("send-reset-email/{email}")]
        public async Task<IActionResult> SendEmail(string email)
        {
            var user = await _db.Users.FirstOrDefaultAsync(a =>a.Email == email);
            if(user is null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "email Doesn't Exist"
                });
            }
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
           
        }
        */


        [HttpGet("UserID/{emailUser}")]
        public IActionResult GetUserById(string emailUser)
        {
            UsersEO user = _db.Users.FirstOrDefault(e => e.Email == emailUser);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }








        // DTO איזור בדיקות 
        [HttpGet]
        [Route("getAllUserDTO")]
        public IActionResult getAllUserDTO()
        {
            List<UsersEO> users = _db.Users.ToList();
            return Ok(users.Select(h => _mapper.Map<UsersDTO>(h)));
        }

        [HttpGet]
        [Route("GetVillaDetails")]
        public IActionResult GetVillaDetails()
        {
            List<VillaDetailsEO> VillaDetail = _db.Details.Where(a => a.NumBedroom > 2).ToList();
            return Ok(VillaDetail.Select(h => _mapper.Map<VillaDetailsDTO>(h)));
        }

        [HttpPost]
        [Route("SetVillaDetails")]
        public IActionResult SetVillaDetails([FromBody] VillaDetailsDTO DetailsDTO)
        {
            var VilaDetaile = _mapper.Map<VillaDetailsEO>(DetailsDTO);
            _db.Details.Add(VilaDetaile);
            _db.SaveChanges();
            return Ok(new
            {
                Message = "וילה נוספה בהצלחה"
            });
        }

        // דוגמה לשאילתה שמקבלת אימייל ומחזירה את השמות של כל הוילות שלו
        [HttpGet]
        [Route("getListOfVilaByEmailUser/{email}")]
        public IActionResult getUserFromDto(string email)
        {
            var userWithVillas = _db.Users
                 .Include(i => i.VillaDetails)
                 .Where(c => c.Email == email)
                 .Select(s => s.VillaDetails.Select(a => a.NameVila));

            if (userWithVillas == null)
            {
                return NotFound("User not found");
            }

            // Map the UserWithVillas object to UserDetailsDTO
            //  var userDto = _mapper.Map<UserDetailsDTO>(userWithVillas);

            return Ok(userWithVillas);
        }


        [HttpPost]
        [Route("AddLineInVillaDetails")]
        public IActionResult AddLineInVillaDetails([FromBody] VillaDetailsEO vilaDetails)
        {

            if (vilaDetails.UserID == 0)
                return BadRequest(new { Message = "נדרש מספר מזהה בעלים  " });

            _db.Details.Add(vilaDetails);
            _db.SaveChanges();
            return Ok(new
            {
                Message = " פרטי וילה נוספו בהצלחה  "
            });
        }

        [HttpPost]
        [Route("AddLineInVillaDescription")]
        public IActionResult AddLineInVillaDescription([FromBody] VillaDescriptionEO vilaDescription)
        {

            if (vilaDescription == null)
                return BadRequest();

            _db.Description.Add(vilaDescription);
            _db.SaveChanges();
            return Ok(new
            {
                Message = " פרטי וילה נוספו בהצלחה  "
            });
        }

        // הוספת וילה
        [HttpPost]
        [Route("VillaDetailsAndDescriptionDTO")]
        public IActionResult VillaDetailsAndDescriptionDTO([FromBody] VillaDetailsAndDescriptionDTO viewModel)
        {

            var mix = _mapper.Map<(VillaDetailsEO, VillaDescriptionEO, CheckListVilaEO, VilaPhoto)>(viewModel);
         //   int currentMaxVilaId = 0;

            if (mix.Item1 == null || mix.Item2 == null)
                return BadRequest();

            if (mix.Item1.UserID == 0)
                return BadRequest(new { Message = "נדרש מספר מזהה בעלים  " });
            int currentMaxVilaId = getMaxVilaIDFromDataBase();

            mix.Item1.VilaId = currentMaxVilaId;
            mix.Item2.VilaId = currentMaxVilaId;
            mix.Item3.VilaId = currentMaxVilaId;



            _db.Details.Add(mix.Item1);
            _db.Description.Add(mix.Item2);
            _db.checkListVila.Add(mix.Item3);
            _db.SaveChanges();

            return Ok();          
        }




        // איזור החזרת ליסט של כל טבלה בנפרד
        [HttpGet("getAllVillasDetail")]
        public IActionResult GetAllVillasDetail()
        {
            List<VillaDetailsEO> listVillaDetails = _db.Details.ToList();
            return Ok(listVillaDetails);                
        }
        [HttpGet("getAllVillasDesctiption")]
        public IActionResult GetAllVillasDesctiption()
        {
            List<VillaDescriptionEO> listVillaDescription = _db.Description.ToList();
            return Ok(listVillaDescription);
        }
        [HttpGet("getAllVillasCheckList")]
        public IActionResult GetAllVillasCheckList()
        {
            List<CheckListVilaEO> listVillaCheckList = _db.checkListVila.ToList();
            return Ok(listVillaCheckList);
        }
        // עד כאן החזרת ליסט של כל טבלה בנפרד










        // מקבל פרטים של וילה לפי מספר וילה
        [HttpGet]
        [Route("getVillaDetailsByVillaID/{vilaId}")]
        public IActionResult getVillaDetailsByVillaID(int vilaId)
        {        
            VillaDetailsEO detail = _db.Details.Find(vilaId);              
            return Ok(detail);
        } 
        [HttpGet]
        [Route("getDescriptionByVillaID/{vilaId}")]
        public IActionResult getDescriptionByVillaID(int vilaId)
        {
            VillaDescriptionEO description = _db.Description.Find(vilaId);
            return Ok(description);
        } 
        [HttpGet]
        [Route("getCheckListByVillaID/{vilaId}")]
        public IActionResult getCheckListByVillaID(int vilaId)
        {;
            CheckListVilaEO checkList = _db.checkListVila.Find(vilaId);
            return Ok(checkList);
        }
        [HttpGet("getImages/{vilaId}")]
        public IActionResult GetImages(int vilaId)
        {
            try
            {
                var images = _db.Photos.Where(v => v.VilaId == vilaId).ToList();

                if (images == null || images.Count == 0)
                {
                    return NotFound("No images found for the specified villa.");
                }

                // Extract file paths from the list of images
                var filePaths = images.Select(image => image.FilePath).ToList();

                // Return the list of file paths
                return Ok(filePaths);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        // מחזיר רק תמונה אחת מכל וילה
        [HttpGet("getFirstImageOfEachVilla")]
        public IActionResult GetFirstImageOfEachVilla()
        {
            try
            {
                var allImages = _db.Photos.ToList();
                // Group images by VilaId and select the first image of each group
                var firstImages = allImages
                    .GroupBy(v => v.VilaId)
                    .Select(group => group.First())
                    .ToList();

                if (firstImages == null || firstImages.Count == 0)
                {
                    return NotFound("No images found for any villa.");
                }

                // Extract file paths from the list of first images
                var filePaths = firstImages.Select(image => image.FilePath).ToList();
                int a = filePaths.Count();
                // Return the list of file paths
                return Ok(filePaths);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        } // עד כאן קבלת פרטים על וילה בעזרת מזהה וילה












        // בדיקת צ'קליסט
        [HttpPost]
        [Route("checklistVila")]
        public IActionResult checklistVila([FromBody] CheckListVilaEO checkList)
        {
            int currentMaxVilaId = _db.checkListVila.Max(d => d.VilaId);
            currentMaxVilaId += 1;
            checkList.VilaId = currentMaxVilaId;
            _db.checkListVila.Add(checkList);
            _db.SaveChanges();

            return Ok(new { Message = "פרטי וילה נוספו בהצלחה" });

        }

        [HttpGet]
        [Route("getchecklistVila")]
        public IActionResult getchecklistVila()
        {
            List<CheckListVilaEO> chack = _db.checkListVila.ToList();
            return Ok(chack);
        }




        [HttpGet]
        [Route("vilaDetails")]
        public IActionResult getvilaDetails()
        {
            List<VillaDetailsEO> vilaDetails = _db.Details.ToList();
            return Ok(vilaDetails);
        }


        // איזור בדיקות קבלת פרטים על משתמש בעזרת הטוקן

        [HttpGet("listVilasByUserID/{UserID}")] // ומחזירה ליסט של כל הוילות שלו userID מקבלת 
        public IActionResult GetUserById(int userID)
        {
            List<VillaDetailsEO> listVila = _db.Details.Where(v => v.UserID == userID).ToList();

            return Ok(listVila);
        }


        /*
        [HttpPost("addImages")]
        public IActionResult AddImages([FromForm] List<IFormFile> images)
        {
            int currentMaxVilaId = _db.Details.Max(d => d.VilaId) + 1;

            try
            {
                foreach (var image in images)
                {
                    if (image != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            image.CopyTo(memoryStream);
                            byte[] imageBytes = memoryStream.ToArray();

                            var photo = new VilaPhoto
                            {
                                VilaId = currentMaxVilaId,
                                FilePath = imageBytes
                            };

                            _db.Photos.Add(photo);
                        }
                    }
                }

                _db.SaveChanges();

                return Ok(new { Message = "פרטי וילה נוספו בהצלחה" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        
        // מחזירה רשימה של תמונות לפי מזהה וילה
        [HttpGet("getPhotoByVilaID/{vilaID}")] 
        public IActionResult getPhotoByVilaID(int vilaID)
        {
            List<VilaPhoto> listPhoto = _db.Photos.Where(v => v.VilaId == vilaID).ToList();
            foreach (var photo in listPhoto)
            {
                photo.FilePath = ConvertImageToBase64(photo.FilePath);
            }
            return Ok(listPhoto);
        }
        */



        // טסטים של תמונות
        /*
        [HttpPost("addImages")]
        public IActionResult AddImages([FromForm] List<IFormFile> images)
        {
            try
            {
                foreach (var image in images)
                {
                    if (image != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            image.CopyTo(memoryStream);
                            byte[] imageBytes = memoryStream.ToArray();

                            var photo = new VilaPhoto
                            {
                                VilaId = 5,
                                FilePath = Convert.ToBase64String(imageBytes) // Convert to base64 for simplicity
                            };

                            _db.Photos.Add(photo);
                        }
                    }
                }

                _db.SaveChanges();

                return Ok(new { Message = "Images uploaded successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        */

        // מקבלת תמונ האחת ומעדכנת אותה זה עובד 
        [HttpPost("addImagesssss")]
        public IActionResult AddImagesToDatabase([FromForm] List<IFormFile> images)
        {
            int currentMaxVilaId = getMaxVilaIDFromDataBase();

            try
            {
                foreach (var image in images)
                {
                    if (image != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            image.CopyTo(memoryStream);
                            byte[] imageBytes = memoryStream.ToArray();

                            var photo = new VilaPhoto
                            {
                                VilaId = currentMaxVilaId,
                                FilePath = imageBytes
                            };

                            _db.Photos.Add(photo);
                        }
                    }
                }

                _db.SaveChanges();

                return Ok(new { Message = "פרטי וילה נוספו בהצלחה" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        

        public int getMaxVilaIDFromDataBase()
        {/*
            if (_db.Details.Count() == 0)
                return 1;   */      
            int currentMaxVilaId = _db.Details.Max(d => d.VilaId) + 1;
            return currentMaxVilaId;
            
            
        }


    }
}

