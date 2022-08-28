using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Fifthyear.Models;

namespace Fifthyear.Data
{
    public class Context : DbContext
    {

        public Context(DbContextOptions<Context> opt)
        : base(opt)
        {

        }
        public DbSet<Indice> indices { get; set; }
        public DbSet<Vacancy> vacancies { get; set; }
        public DbSet<VacancyEducation> vacancyEducations { get; set; }
        public DbSet<VacancyLanguage> vacancyLanguages { get; set; }
        public DbSet<VacancySkill> vacancySkills { get; set; }
        public DbSet<VacancyWorkExperience> vacancyWorkExperiences { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<JobSeeker> jobSeekers { get; set; }
        public DbSet<Employeer> employeers { get; set; }
        public DbSet<JobSeekerEducation> jobSeekerEducations { get; set; }
        public DbSet<JobSeekerLanguage> jobSeekerLanguages { get; set; }
        public DbSet<JobSeekerProfile> JobSeekerProfiles { get; set; }
        public DbSet<JobSeekerWorkExperience> jobSeekerWorkExperiences { get; set; }
        public DbSet<JobSeekerSkill> jobSeekerSkills { get; set; }
        public DbSet<PreviousJob> previousJobs { get; set; }
        public DbSet<EmployeerBranch> employeerBranches { get; set; }
        public DbSet<EmployeerProfile> employeerProfiles { get; set; }
        public DbSet<MarkedVacancy> markedVacancies { get; set; }
        public DbSet<Reusme> reusmes { get; set; }
        public DbSet<ReusmeEducation> reusmeEducations { get; set; }
        public DbSet<ReusmeSkill> reusmeSkills { get; set; }
        public DbSet<ReusmeWorkExperience> reusmeWorkExperiences { get; set; }
        public DbSet<NotificationResponse> notifications { get; set; }
        public DbSet<EmployeerEmail> employeerEmails { get; set; }
        public DbSet<AlgorithmResult> algorithmResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach(var foreignKey in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e=>e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.SetNull;
            }
          
           
            

            modelBuilder.Entity<Indice>()

              .HasData(
              new Indice { ID = 1 , Module=  0, Value= "Communication"                                                },
              new Indice { ID = 2 , Module = 0, Value = "Teamwork"                                                    },
              new Indice { ID = 3 , Module = 0, Value = "dependability"                                               },
              new Indice { ID = 4 , Module = 0, Value = "Problem-solving"                                             },
              new Indice { ID = 5 , Module = 0, Value = "Collecting Information"                                      },
              new Indice { ID = 6 , Module = 0, Value = "Attention to detail"                                         },
              new Indice { ID = 7 , Module = 0, Value = "adaptability"                                                },
              new Indice { ID = 8 , Module = 0, Value = "Strategic Planning"                                          },
              new Indice { ID = 9 , Module = 0, Value = "motivation"                                                  },
              new Indice { ID = 10, Module = 0, Value = "Stress Tolerance"                                            },
              new Indice { ID = 11, Module = 0, Value = "Positive attitude"                                           },
              new Indice { ID = 12, Module = 0, Value = "Flexibility"                                                 },
              new Indice { ID = 13, Module = 0, Value = "Willingness to learn"                                        },
              new Indice { ID = 14, Module = 0, Value = "Self-management"                                             },
              new Indice { ID = 15, Module = 0, Value = "decision making"                                             },
              new Indice { ID = 16, Module = 0, Value = "Resilience"                                                  },
              new Indice { ID = 17, Module = 0, Value = "Empathy"                                                     },
              new Indice { ID = 18, Module = 0, Value = "Integrity"                                                   },
              new Indice { ID = 19, Module = 0, Value = "Open-mindedness"                                             },
              new Indice { ID = 20, Module = 0, Value = "Creativity"                                                  },
              new Indice { ID = 21, Module = 0, Value = "Organization"                                                },
              new Indice { ID = 22, Module = 0, Value = "Numerical Skills "                                           },
              new Indice { ID = 23, Module = 0, Value = "Strategic Thinking"                                          },
              new Indice { ID = 24, Module = 0, Value = "The ability to make difficult concepts easy to understand"   },
              new Indice { ID = 25, Module = 0, Value = "Relationship Management"                                     },
              new Indice { ID = 26, Module = 0, Value = "Friendliness"                                                },
              new Indice { ID = 27, Module = 0, Value = "Confidence"                                                  },
              new Indice { ID = 28, Module = 0, Value = "Storytelling"                                                },
              new Indice { ID = 29, Module = 0, Value = "Credibility"                                                 },
              new Indice { ID = 30, Module = 0, Value = "Maintaining eye contact"                                     },
              new Indice { ID = 31, Module = 0, Value = "Active Listening"                                            },
              new Indice { ID = 32, Module = 0, Value = "Politeness"                                                  },
              new Indice { ID = 33, Module = 0, Value = "Maintaining Calm Under Pressure"                             },
              new Indice { ID = 34, Module = 0, Value = "Knowing When to Ask Questions"                               },
              new Indice { ID = 35, Module = 0, Value = "Negotiation"                                                 },
              new Indice { ID = 36, Module = 0, Value = "Showing assertiveness"                                       },
              new Indice { ID = 37, Module = 0, Value = "Focus"                                                       },
              new Indice { ID = 38, Module = 0, Value = "Emotional intelligence"                                      },
              new Indice { ID = 39, Module = 0, Value = "Paying attention to the non-verbal communication of others"  },
              new Indice { ID = 40, Module = 0, Value = "Challenge assumptions in a polite manner"                    },
              new Indice { ID = 41, Module = 0, Value = "Critical Thinking"                                           },




              new Indice { ID = 42, Module = 1, Value = "University of Oxford"                                        },
              new Indice { ID = 43, Module = 1, Value = "Stanford University"                                         },
              new Indice { ID = 44, Module = 1, Value = "Harvard University"                                          },
              new Indice { ID = 45, Module = 1, Value = "California Institute of Technology"                          },
              new Indice { ID = 46, Module = 1, Value = "Imperial College London"                                     },
              new Indice { ID = 47, Module = 1, Value = "ETH Zurich"                                                  },
              new Indice { ID = 48, Module = 1, Value = "University of Chicago"                                       },
              new Indice { ID = 49, Module = 1, Value = "National University of Singapore"                            },
              new Indice { ID = 50, Module = 1, Value = "Nanyang Technological University"                            },
              new Indice { ID = 51, Module = 1, Value = "University of Pennsylvania"                                  },
              new Indice { ID = 52, Module = 1, Value = "Yale University"                                             },
              new Indice { ID = 53, Module = 1, Value = "University of Edinburgh"                                     },
              new Indice { ID = 54, Module = 1, Value = "Tsinghua University"                                         },
              new Indice { ID = 55, Module = 1, Value = "Peking University"                                           },
              new Indice { ID = 56, Module = 1, Value = "Columbia University"                                         },
              new Indice { ID = 57, Module = 1, Value = "Princeton University"                                        },
              new Indice { ID = 58, Module = 1, Value = "Cornell University"                                          },
              new Indice { ID = 59, Module = 1, Value = "University of Hong Kong"                                     },
              new Indice { ID = 60, Module = 1, Value = "University of Tokyo"                                         },
              new Indice { ID = 61, Module = 1, Value = "University of Michigan-Ann Arbor"                            },
              new Indice { ID = 62, Module = 1, Value = "Johns Hopkins University"                                    },
              new Indice { ID = 63, Module = 1, Value = "University of Toronto"                                       },
              new Indice { ID = 64, Module = 1, Value = "McGill University"                                           },
              new Indice { ID = 65, Module = 1, Value = "Australian National University"                              },
              new Indice { ID = 66, Module = 1, Value = "University of Manchester"                                    },
              new Indice { ID = 67, Module = 1, Value = "Northwestern University"                                     },
              new Indice { ID = 68, Module = 1, Value = "Fudan University"                                            },
              new Indice { ID = 69, Module = 1, Value = "University of California, Berkeley"                          },
              new Indice { ID = 70, Module = 1, Value = "Kyoto University"                                            },
              new Indice { ID = 71, Module = 1, Value = "Hong Kong University of Science and Technology"              },
              new Indice { ID = 72, Module = 1, Value = "King's College London"                                       },
              new Indice { ID = 73, Module = 1, Value = "Seoul National University"                                   },
              new Indice { ID = 74, Module = 1, Value = "University of Melbourne"                                     },
              new Indice { ID = 75, Module = 1, Value = "University of Sydney"                                        },
              new Indice { ID = 76, Module = 1, Value = "Chinese University of Hong Kong"                             },
              new Indice { ID = 77, Module = 1, Value = "Korea Advanced Institute of Science & Technology"            },
              new Indice { ID = 78, Module = 1, Value = "New York University"                                         },
              new Indice { ID = 79, Module = 1, Value = "University of New South Wales"                               },
              new Indice { ID = 80, Module = 1, Value = "Universite PSL"                                              },
              new Indice { ID = 81, Module = 1, Value = "Zhejiang University"                                         },
              new Indice { ID = 82, Module = 1, Value = "University of Illinois at Urbana-Champaign"                  },
              new Indice { ID = 83, Module = 1, Value = "University of Auckland"                                      },
              new Indice { ID = 84, Module = 1, Value = "University of Washington"                                    },
              new Indice { ID = 85, Module = 1, Value = "Lund University"                                             },
              new Indice { ID = 86, Module = 1, Value = "Georgia Institute of Technology"                             },
              new Indice { ID = 87, Module = 1, Value = "University of Birmingham"                                    },



              new Indice { ID = 88 , Module = 2, Value = "Computer & Information Sciences, General" },
              new Indice { ID = 89, Module = 2, Value = "Computer Networking/Telecommunications" },
              new Indice { ID = 90, Module = 2, Value = "Computer Science & Programming" },
              new Indice { ID = 91, Module = 2, Value = "Computer Software & Media Applications" },
              new Indice { ID = 92, Module = 2, Value = "Computer System Administration" },
              new Indice { ID = 93, Module = 2, Value = "Data Management Technology" },
              new Indice { ID = 94, Module = 2, Value = "Information Science" },
              new Indice { ID = 95, Module = 2, Value = "Webpage Design" },
              new Indice { ID = 96, Module = 2, Value = "Mathematics, General" },
              new Indice { ID = 97, Module = 2, Value = "Applied Mathematics" },
              new Indice { ID = 98, Module = 2, Value = "Statistics" },
              new Indice { ID = 99, Module = 2, Value = "Counseling & Student Services" },
              new Indice { ID = 100, Module = 2, Value = "Chiropractic (Pre-Chiropractic)" },
              new Indice { ID = 101, Module = 2, Value = "Dental Hygiene" },
              new Indice { ID = 102, Module = 2, Value = "Dentistry (Pre-Dentistry)" },
              new Indice { ID = 103, Module = 2, Value = "Emergency Medical Technology" },
              new Indice { ID = 104, Module = 2, Value = "Health-Related Professions & Services, General" },
              new Indice { ID = 105, Module = 2, Value = "Public Health" },
              new Indice { ID = 106, Module = 2, Value = "Health/Medical Technology, General" },
              new Indice { ID = 107, Module = 2, Value = "Medical Laboratory Technology" },
              new Indice { ID = 108, Module = 2, Value = "Surgical Technology" },
              new Indice { ID = 109, Module = 2, Value = "Medicine (Pre-Medicine)" },
              new Indice { ID = 110, Module = 2, Value = "Nursing, Practical/Vocational (LPN)" },
              new Indice { ID = 111, Module = 2, Value = "Nursing, Registered (BS/RN)" },
              new Indice { ID = 112, Module = 2, Value = "Physician Assisting" },
              new Indice { ID = 113, Module = 2, Value = "Pharmacy (Pre-Pharmacy)" },
              new Indice { ID = 114, Module = 2, Value = "Osteopathic Medicine" },
              new Indice { ID = 115, Module = 2, Value = "Optometry (Pre-Optometry)" },
              new Indice { ID = 116, Module = 2, Value = "Engineering (Pre-Engineering), General" },
              new Indice { ID = 117, Module = 2, Value = "Aerospace/Aeronautical Engineering" },
              new Indice { ID = 118, Module = 2, Value = "Agricultural/Bioengineering" },
              new Indice { ID = 119, Module = 2, Value = "Architectural Engineering" },
              new Indice { ID = 120, Module = 2, Value = "Biomedical Engineering" },
              new Indice { ID = 121, Module = 2, Value = "Chemical Engineering" },
              new Indice { ID = 122, Module = 2, Value = "Civil Engineering" },
              new Indice { ID = 123, Module = 2, Value = "Construction Engineering/Management" },
              new Indice { ID = 124, Module = 2, Value = "Electrical, Electronics & Communications Engineering" },
              new Indice { ID = 125, Module = 2, Value = "Environmental Health Engineering" },
              new Indice { ID = 126, Module = 2, Value = "Industrial Engineering" },
              new Indice { ID = 127, Module = 2, Value = "Mechanical Engineering" },
              new Indice { ID = 128, Module = 2, Value = "Nuclear Engineering" },
              new Indice { ID = 129, Module = 2, Value = "Health and Safety Engineering" },
              new Indice { ID = 130, Module = 2, Value = "Accounting" },
              new Indice { ID = 131, Module = 2, Value = "Accounting Technician" },
              new Indice { ID = 132, Module = 2, Value = "Educational Administration" },
              new Indice { ID = 133, Module = 2, Value = "Teacher Education, General" },
              new Indice { ID = 134, Module = 2, Value = "Teacher Assisting/Aide Education" },
              new Indice { ID = 135, Module = 2, Value = "Art Education" },
              new Indice { ID = 136 , Module = 2, Value = "Business Education" },
              new Indice { ID = 137, Module = 2, Value = "Drafting/CAD Technology, General" },              
              new Indice { ID = 138 , Module = 2, Value = "Architectural Drafting/CAD Technology" },                        
              new Indice { ID = 139, Module = 2, Value = "Mechanical Drafting/CAD Technology" },
              new Indice { ID = 140, Module = 2, Value = "Quality Control & Safety Technologies" },
              new Indice { ID = 141, Module = 2, Value = "Surveying Technology" },
              new Indice { ID = 142, Module = 2, Value = "English Language & Literature, General" },
              new Indice { ID = 143, Module = 2, Value = "American/English Literature" },
              new Indice { ID = 144, Module = 2, Value = "Creative Writing" },
              new Indice { ID = 145, Module = 2, Value = "Public Speaking" },
              new Indice { ID = 146, Module = 2, Value = "Foreign Languages/Literatures, General" },
              new Indice { ID = 147, Module = 2, Value = "Asian Languages & Literatures" },
              new Indice { ID = 148, Module = 2, Value = "Classical/Ancient Languages & Literatures" },
              new Indice { ID = 149, Module = 2, Value = "Comparative Literature" },
              new Indice { ID = 150, Module = 2, Value = "French Language & Literature" },
              new Indice { ID = 151, Module = 2, Value = "German Language & Literature" },
              new Indice { ID = 152, Module = 2, Value = "Linguistics" },
              new Indice { ID = 153, Module = 2, Value = "Middle Eastern Languages & Literatures" },
              new Indice { ID = 154, Module = 2, Value = "Spanish Language & Literature" },
              new Indice { ID = 155, Module = 2, Value = "Business Administration & Management, General" },
              new Indice { ID = 156, Module = 2, Value = "Hotel/Motel Management" },
              new Indice { ID = 157, Module = 2, Value = "Mining and Geological Engineering" },
              new Indice { ID = 158, Module = 2, Value = "Sales, Merchandising, & Marketing, General" },
              new Indice { ID = 159, Module = 2, Value = "Fashion Merchandising" },
              new Indice { ID = 160, Module = 2, Value = "Digital Communications/Media" },
              new Indice { ID = 161, Module = 2, Value = "Public Relations & Organizational Communication" },
              new Indice { ID = 162, Module = 2, Value = "Radio & Television Broadcasting" },
              new Indice { ID = 163, Module = 2, Value = "Family & Consumer Sciences, General" },
              new Indice { ID = 164, Module = 2, Value = "Child Care Services Management" }

           );

        }
    }
}
