using Newtonsoft.Json;
using JobPortal.Models;

namespace JobPortal.Services
{
    public class SkillMatchingService
    {
        public double CalculateMatchScore(string? applicantSkills, string? jobSkills)
        {
            try
            {
                var applicantSkillIds = JsonConvert.DeserializeObject<List<int>>(applicantSkills ?? "[]") ?? new List<int>();
                var jobSkillIds = JsonConvert.DeserializeObject<List<int>>(jobSkills ?? "[]") ?? new List<int>();

                if (jobSkillIds.Count == 0) return 0;

                var matchingSkills = applicantSkillIds.Intersect(jobSkillIds).Count();
                return Math.Round((double)matchingSkills / jobSkillIds.Count * 100, 2);
            }
            catch
            {
                return 0;
            }
        }

        public List<int> GetSkillIds(string? skillsJson)
        {
            try
            {
                return JsonConvert.DeserializeObject<List<int>>(skillsJson ?? "[]") ?? new List<int>();
            }
            catch
            {
                return new List<int>();
            }
        }
    }
}