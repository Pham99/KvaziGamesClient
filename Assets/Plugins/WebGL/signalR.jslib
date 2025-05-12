mergeInto(LibraryManager.library, {
    StartConnection: function(serverURL) {
        var serverURLstr = UTF8ToString(serverURL);
        Connect(serverURLstr);
    }
});