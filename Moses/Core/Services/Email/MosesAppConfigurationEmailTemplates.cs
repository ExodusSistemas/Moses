using System;
using System.Collections.Generic;
using System.Text;

namespace Moses.Services.Email
{
    public class MosesAppConfigurationEmailTemplates
    {
        public MosesEmailTemplate SystemEmail { get; set; } = new MosesEmailTemplate() { SubjectTag = "[System]" };
        public MosesEmailTemplate BugTrackEmail { get; set; } = new MosesEmailTemplate() { SubjectTag = "[Bug]" };
        public MosesEmailTemplate SupportEmail { get; set; } = new MosesEmailTemplate() { SubjectTag = "[Support]" };
        public MosesEmailTemplate ErrorEmail { get; set; } = new MosesEmailTemplate() { SubjectTag = "[Error]" };
        public MosesEmailTemplate ChangePasswordEmail { get; set; } = new MosesEmailTemplate() { SubjectTag = "[Change Password]" };
    }
}
