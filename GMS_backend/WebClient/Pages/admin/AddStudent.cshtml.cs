using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebClient.DTO.SessionStudent;
using WebClient.DTO.User;
using WebClient.Services;
using System.Collections.Generic;
using System.Linq;

namespace WebClient.Pages.admin
{
    public class AddStudentModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int SessionId { get; set; }

        public List<GetStudentBySession> CurrentStudents { get; set; }
        public List<GetUserDTO> AllStudents { get; set; }

        [BindProperty]
        public List<int> SelectedStudentIds { get; set; } = new List<int>();

        public List<GetUserDTO> SelectedStudents =>
            AllStudents?.Where(s => SelectedStudentIds.Contains(s.Id)).ToList();

        public void OnGet()
        {
            System.Diagnostics.Debug.WriteLine($"OnGet - SessionId: {SessionId}");

            CurrentStudents = SessionStudentService.GetSessionStudentsBySessionId(SessionId);
            AllStudents = UserService.GetUsersByRole(4);
        }

        public IActionResult OnPostAddStudents()
        {
            System.Diagnostics.Debug.WriteLine($"OnPostAddStudents - SessionId: {SessionId}");
            System.Diagnostics.Debug.WriteLine($"OnPostAddStudents - SelectedStudentIds: {string.Join(", ", SelectedStudentIds)}");

            if (SelectedStudentIds.Any())
            {
                foreach (var studentId in SelectedStudentIds)
                {
                    var newStudent = new SessionStudentDTO
                    {
                        SessionId = SessionId,
                        StudentId = studentId
                    };

                    SessionStudentService.AddStudentToSession(newStudent);
                }

                return RedirectToPage(new { SessionId });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Vui lòng chọn ít nhất một sinh viên.");
            }

            CurrentStudents = SessionStudentService.GetSessionStudentsBySessionId(SessionId);
            AllStudents = UserService.GetUsersByRole(4);

            return Page();
        }
    }
}
