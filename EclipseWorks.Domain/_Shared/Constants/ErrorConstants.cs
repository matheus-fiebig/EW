namespace EclipseWorks.Domain._Shared.Constants
{
    public static class ErrorConstants
    {
        public const string TitleNullCode = "000001";
        public const string TitleNullDesc = "Titulo vazio ou nulo.";

        public const string DescriptionNullCode = "000002";
        public const string DescriptionNullDesc = "Descrição vazia ou nula.";

        public const string InvalidDueDateCode = "000003";
        public const string InvalidDueDateDesc = "Data de Vencimento vazia ou nula.";

        public const string PriorityNullCode = "000004";
        public const string PriorityNullDesc = "Prioridade nula.";

        public const string ProjectNotFoundCode = "000005";
        public const string ProjectNotFoundDesc = "Projeto não encontrado.";

        public const string DueDateNotAllowedCode = "000006";
        public const string DueDateNotAllowedDesc = "Data de Vencimento deve ser superior ao dia de hoje.";

        public const string ProgressNullCode = "000007";
        public const string ProgressNullDesc = "O progresso atual da tarefa deve ser informado.";

        public const string TaskNotFoundCode = "000008";
        public const string TaskNotFoundDesc = "Tarefa não encontrada.";

        public const string CommentaryNullCode = "000009";
        public const string CommentaryNullDesc = "Comentário vazio ou nulo.";

        public const string UserNotFoundCode = "000010";
        public const string UserNotFoundDesc = "Usuário não encontrado.";

        public const string ProjectMinUserCode = "000011";
        public const string ProjectMinUserDesc = "Numero de usuários invalido. Min: 1.";

        public const string ProjectDeletionCode = "000012";
        public const string ProjectDeletionDesc = "Existem tarefas pendentes nesse projeto. E necessário conclui-las ou remove-las antes de prosseguir.";

        public const string TaskLimitExceededCode = "000013";
        public const string TaskLimitExceededDesc = "Limite de tarefas atingido para este projeto (20).";

        public const string NullStartDateCode = "000014";
        public const string NullStartDateDesc = "Data inicial deve ser informada.";

        public const string NullEndDateCode = "000015";
        public const string NullEndDateDesc = "Data final deve ser informada.";

        public const string EndDateLowerThanStartingDateCode = "000016";
        public const string EndDateLowerThanStartingDateDesc = "Data final não pode ser menor que a da inicial.";

        public const string NotAuthorizedCode = "000017";
        public const string NotAuthorizedDesc = "Sem permissão para realizar essa ação.";

        public const string InvalidRequestCode = "000099";
        public const string InvalidRequestDesc = "Requisição invalida.";
    }
}
