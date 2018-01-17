namespace LiveDirectorySyncEngineLogic.SyncActionModel
{
    public interface ISyncActionHandler
    {
        void Create(SyncCreateActionCommand command);
        void Rename(SyncRenameActionCommand command);
        void Update(SyncUpdateActionCommand command);
        void Delete(SyncDeleteActionCommand command);
        void CanStart();
    }
}
