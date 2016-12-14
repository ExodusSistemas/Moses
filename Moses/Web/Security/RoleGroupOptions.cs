using System.ComponentModel;

namespace Moses.Security
{

    public enum RoleGroupOptions
    {
        [Description("N/A")]
        None = 0,

        [Description("Proprietários")]
        Owners = 10,

        [Description("Auditores")]
        Auditors = 15,

        [Description("Diretores")]
        Directors = 20,

        [Description("Superintendentes")]
        Superintendents = 30,

        [Description("Gerentes")]
        Managers = 40,

        [Description("Coordenadores")]
        Coordinators = 50,

        [Description("Supervisores")]
        Supervisors = 60,

        [Description("Analistas")]
        Analysts = 70,

        [Description("Operativos")]
        Operatives = 80,

        [Description("Credenciados")]
        Acredited = 85,

        [Description("CredenciadosOperativos")]
        AcreditedOperatives = 90,

        [Description("Administradores de Sistema")]
        SystemAdmin = 99,
    }
}
