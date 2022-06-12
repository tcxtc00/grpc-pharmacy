using Grpc.Core;

namespace HelloGrpc
{
    public class PharmacyService : Medicine.MedicineBase
    {
        private readonly ILogger<PharmacyService> _logger;
        public PharmacyService(ILogger<PharmacyService> logger)
        {
            _logger = logger;
        }

       public override async Task GetMedicine(MedicineRequest request, 
       IServerStreamWriter<MedicineResponse> responseStream, ServerCallContext context)
       {
           try
           {
               List<MedicineResponse> medicines = GetAllMedicines();
               MedicineResponse response = medicines.FirstOrDefault(m => m.Name == request.Name);
               response.Quantity = request.Quantity;
               response.Supplies = response.Supplies - request.Quantity;
               response.ErrorMsg = "Success";
                
               await responseStream.WriteAsync(medicines.FirstOrDefault(m => m.Name == request.Name));
           }
           catch (System.Exception e)
           {
                MedicineResponse response = new MedicineResponse{
                    Name = request.Name,
                    Quantity = request.Quantity,
                    ErrorMsg = e.Message
                };

                if(response.ErrorMsg == "Object reference not set to an instance of an object.")
                {
                    response.ErrorMsg = "No Medicine available with name: " + response.Name;
                }

                await responseStream.WriteAsync(response);
           }
       }


        private List<MedicineResponse> GetAllMedicines()
        {
            //This is hardcoded but realistically you would get 
            //this from a database.

           MedicineResponse medicine1 = new MedicineResponse();
           medicine1.Name = "Bromazepan";
           medicine1.Supplies = 150;
          
           MedicineResponse medicine2 = new MedicineResponse();
           medicine2.Name = "Brufen";
           medicine2.Supplies = 240;
           
           MedicineResponse medicine3 = new MedicineResponse();
           medicine3.Name = "Acisal";
           medicine3.Supplies = 135;

           MedicineResponse medicine4 = new MedicineResponse();
           medicine4.Name = "Ksanaks";
           medicine4.Supplies = 120;

           List<MedicineResponse> medicines = new List<MedicineResponse>();
           medicines.Add(medicine1);
           medicines.Add(medicine2);
           medicines.Add(medicine3);
           medicines.Add(medicine4);

           return medicines;
        }
    }
}
