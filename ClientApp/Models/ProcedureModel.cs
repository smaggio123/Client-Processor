namespace ClientApp.Models
{
    public class ProcedureModel
    {
        public int Procedure_ID { get; } = -1;
        public string ProcedureName { get; } = string.Empty;
        public string procedureDate{ get; } = string.Empty;
        public int Cleint_ID { get; } = -1;
        public int EmployeeID { get; } = -1;
        public string procedureNotes { get; } = string.Empty;

        ProcedureModel()
        {

        }

        public ProcedureModel(int procedure_ID, string procedureName, string procedureDate, int cleint_ID, int employeeID, string procedureNotes)
        {
            this.Procedure_ID = procedure_ID;
            this.ProcedureName = procedureName;
            this.procedureDate = procedureDate;
            this.procedureNotes = procedureNotes;
            this.Cleint_ID = cleint_ID;
            this.EmployeeID = employeeID;
        }
    }
}
