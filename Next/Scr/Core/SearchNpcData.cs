using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next
{
    public class SearchNpcData
    {
        public int ID { get; set; }
        public int ImportantID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int AgeYear { get; set; }
        public int Gender { get; set; }
        public int Level { get; set; }
        public string LocationName { get; set; }
        public int School { get; set; }
        public int Life { get; set; }
        public int Sprite { get; set; }
        public bool IsCouple { get; set; }
        public bool IsTeacher { get; set; }
        public bool IsStudent { get; set; }
        public bool IsBrother { get; set; }

        public bool FilterCheck(string filter)
        {
            var conditions = filter.Split(' ');
            foreach (var condition in conditions)
            {
                if (!Check(condition))
                    return false;
            }

            return true;
        }

        public bool Check(string matchStr)
        {
            if (string.IsNullOrWhiteSpace(matchStr))
                return true;
            
            if (ID.ToString().Contains(matchStr))
                return true;
            
            if (ImportantID.ToString().Contains(matchStr))
                return true;
            
            if (Name.Contains(matchStr))
                return true;
            
            if (Title.Contains(matchStr))
                return true;
            
            if (LocationName.Contains(matchStr))
                return true;
            
            if (DialogAnalysis.GetSchoolName(School.ToString()).Contains(matchStr))
                return true;

            if (DialogAnalysis.GetGenderName(Gender).Contains(matchStr))
                return true;
            
            if (DialogAnalysis.GetLevelName(Level).Contains(matchStr))
                return true;
            
            return false;
        }

        public override string ToString()
        {
            if (ImportantID != 0)
                return $"{ID} {Name} ({ImportantID})";
            return $"{ID} {Name}"; 
        }

        public void Refresh()
        {
            var jsonData = DialogAnalysis.GetNpcJsonData(ID);
            
            Name = DialogAnalysis.GetNpcName(ID);
            ImportantID = jsonData["BindingNpcID"]?.I ?? 0;
            Title = DialogAnalysis.GetNpcTitle(ID);
            Gender = DialogAnalysis.GetNpcSex(ID);
            AgeYear = DialogAnalysis.GetNpcAge(ID) / 12;
            Level = DialogAnalysis.GetNpcLevel(ID);
            LocationName = DialogAnalysis.GetNpcLocationName(ID);
            Life = DialogAnalysis.GetNpcLife(ID);
            Sprite = DialogAnalysis.GetNpcSprite(ID);
            School = DialogAnalysis.GetNpcSchool(ID);
            IsCouple = DialogAnalysis.IsPlayerCouple(ID);
            IsTeacher = DialogAnalysis.IsPlayerTeacher(ID);
            IsStudent = DialogAnalysis.IsPlayerStudent(ID);
            IsBrother = DialogAnalysis.IsPlayerBrother(ID);
        }
    }
}