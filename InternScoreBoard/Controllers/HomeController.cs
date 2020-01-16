using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InternScoreBoard.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace InternScoreBoard.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadData(List<IFormFile> uploadedFiles, string submit)
        {
            if (ModelState.IsValid)
            {
                if (uploadedFiles.Count > 0)
                {
                    List<InternMarksDataModel> internMarksDatas = new List<InternMarksDataModel>();
                    List<InternDetails> internDetails = new List<InternDetails>();

                    foreach (var file in uploadedFiles)
                    {
                        if (file.FileName.EndsWith(".csv"))
                        {
                            //using var reader = new StreamReader(file.OpenReadStream());
                            using(StreamReader reader = new StreamReader(file.OpenReadStream()))
                            {
                                var lines = reader.ReadToEnd();

                                if (!lines.Equals(""))
                                {
                                    var singleLine = lines.Split("\r\n");

                                    switch (submit)
                                    {
                                        case "all":
                                            for (int i = 1; i < singleLine.Length; i++)
                                            {
                                                if (internDetails.Any(intern => intern.InternName.Equals(singleLine[i].Split(',').First())))
                                                {
                                                    internDetails.Where(intern => intern.InternName.Equals(singleLine[i].Split(',').First())).First().Courses.Add(new CourseMarks
                                                    {
                                                        CourseName = Path.GetFileNameWithoutExtension(file.FileName),
                                                        Percentage = singleLine[i].Split(',').Last()
                                                    });
                                                }
                                                else
                                                {
                                                    InternDetails intern = new InternDetails
                                                    {
                                                        InternName = singleLine[i].Split(',').First(),
                                                        Email = singleLine[i].Split(',').Skip(1).First(),
                                                        Courses = new List<CourseMarks>
                                            {
                                                new CourseMarks
                                                {
                                                    CourseName = Path.GetFileNameWithoutExtension(file.FileName),
                                                    Percentage = singleLine[i].Split(',').Last()
                                                }
                                            }
                                                    };
                                                    internDetails.Add(intern);
                                                }
                                            }
                                            break;

                                        case "individual":
                                            InternMarksDataModel internMarks = new InternMarksDataModel()
                                            {
                                                CourseName = Path.GetFileNameWithoutExtension(file.FileName),
                                                ColumnNames = singleLine[0].Split(',').ToList()
                                            };

                                            internMarks.InternNameAndScores = new List<List<string>>();

                                            for (int i = 1; i < singleLine.Length; i++)
                                            {
                                                internMarks.InternNameAndScores.Add(singleLine[i].Split(',').ToList());
                                            }
                                            internMarksDatas.Add(internMarks);
                                            break;
                                    }
                                }
                                else
                                    ModelState.AddModelError(string.Empty, "No data found");

                                reader.Close();
                            }
                        }
                        else
                            ModelState.AddModelError(string.Empty, "Wrong file format.");
                    }

                    foreach (var detail in internDetails)
                    {
                        foreach (var score in detail.Courses)
                        {
                            detail.AllOverScore += double.Parse(score.Percentage.Remove(score.Percentage.Length - 1, 1));
                        }
                        detail.AllOverScore /= detail.Courses.Count;
                    }

                    if (internDetails.Count > 0)
                    {
                        ViewData["CourseColumnCount"] = (uploadedFiles.Count) * 2;
                        return View("index", internDetails.OrderByDescending(c => c.AllOverScore));
                    }
                    else if (internMarksDatas.Count > 0)
                    {
                        return View("IndividualReport", internMarksDatas);
                    }
                }
                else
                    ModelState.AddModelError(string.Empty, "No file(s) chosen.");
            }
            return View("index");
        }

        [HttpGet]
        public IActionResult IndividualReport()
        {
            return View();
        }
    }
}
