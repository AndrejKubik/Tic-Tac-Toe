mergeInto(LibraryManager.library, {
    SyncFiles: function () {
        FS.syncfs(false, function (err) {
            if (err) {
                console.error("FS sync failed:", err);
            } else {
                console.log("FS sync successful");
            }
        });
    }
});