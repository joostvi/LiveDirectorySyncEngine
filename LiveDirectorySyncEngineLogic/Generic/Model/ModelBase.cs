
namespace LiveDirectorySyncEngineLogic.Generic.Model
{
    public class ModelBase : IIdentifier
    {
        public int Id { get; set; }

        public ModelBase(int id)
        {
            Id = id;
        }
    }
}
