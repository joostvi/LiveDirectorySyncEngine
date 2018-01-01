namespace LiveDirectorySyncEngineLogic.SyncActionModel
{
    public interface ISyncAction
    {
        void Create(SyncCreateActionCommand command);
        void Rename(SyncRenameActionCommand command);
        void Update(SyncUpdateActionCommand command);
        void Delete(SyncDeleteActionCommand command);
    }
}
