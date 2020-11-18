using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHub.University
{
    public class UniversityEssence
    {
        public string STUDENT_ROLE = "student";
        public string DEANERY_ROLE = "deanery";
        public string TEACHER_ROLE = "teacher";
        private static UniversityEssence instance;
        public int[] courses = new int[5];
        public int[] groups = new int[10];
        public int[] notes = new int[10];
        public int[] countOfGaps = new int[30];

        private UniversityEssence()
        {
            for (int i = 0; i < 5; i++)
            {
                courses[i] = i + 1;
            }

            for (int i = 0; i < 10; i++)
            {
                groups[i] = i + 1;
            }

            for (int i = 0; i < 10; i++)
            {
                notes[i] = i + 1;
            }

            for (int i = 0; i < 30; i++)
            {
                countOfGaps[i] = i + 1;
            }
        }

        public static UniversityEssence GetInstance()
        {
            return instance ?? (instance = new UniversityEssence());
        }

    }
}
